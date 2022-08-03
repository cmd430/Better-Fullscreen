Imports System.Threading

Public Class BetterFullscreen

    Private ReadOnly WindowsScaleFactor As Integer = GetWindowsScaleFactor()
    Private ReadOnly Hotkeys As New Hotkeys
    Private ReadOnly TaskSchedeuler As New Scheduler("Better Fullscreen", "cmd430", "Starts Better Fullscreen on Logon", Nothing, Nothing, False)
    Private ReadOnly WinEventProcDelegate As New WinEventDelegate(AddressOf WinEventProc)
    Private ReadOnly WinEventHook As IntPtr = SetWinEventHook(WIN_EVENT.EVENT_SYSTEM_FOREGROUND, WIN_EVENT.EVENT_SYSTEM_CAPTURESTART, IntPtr.Zero, WinEventProcDelegate, 0, 0, WIN_EVENT_FLAGS.WINEVENT_OUTOFCONTEXT)
    Private ReadOnly ConfigPath As String = Application.ExecutablePath.Replace(".exe", ".conf")
    Private ReadOnly Config As BetterFullscreenConfig = LoadConfig(ConfigPath)

    Private CloseSilently As Boolean = False
    Private DebounceTrigger As New KeyValuePair(Of Date, IntPtr)(Now, IntPtr.Zero)

#Region "Form Events"

    Private Sub BetterFullscreen_Load(sender As Object, e As EventArgs) Handles Me.Load
        AddHandler Hotkeys.KeyPressed, AddressOf AddGame

        Hotkeys.RegisterHotKey(Config.Settings.Modifier, Config.Settings.Hotkey)

        If TaskSchedeuler.GetTask() Is Nothing Then
            TaskSchedeuler.AddTask()
            LogEvent("Added startup task")
        Else
            TaskSchedeuler.UpdateTask()
            LogEvent("Updated startup task")
        End If

        If LoadWithWindows() Then
            CheckBox_StartWithWindows.Checked = True
        End If

        AddHandler CheckBox_StartWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged

        If GetWindowsTheme() = WindowsTheme.Light Then
            Icon = My.Resources.Fullscreen_Dark
            TrayIcon.Icon = My.Resources.Fullscreen_Dark
        Else
            Icon = My.Resources.Fullscreen_Light
            TrayIcon.Icon = My.Resources.Fullscreen_Light
            SendMessage(Handle, WIN_MESSAGE.WM_SETICON, ICON_SIZE.ICON_SMALL, My.Resources.Fullscreen_Dark.Handle)
        End If

        ToggleWindowState(Me, IIf(Debugger.IsAttached Or Not Config.Settings.StartHidden, FormWindowState.Normal, FormWindowState.Minimized))

        Init()
    End Sub

    Protected Overrides Sub SetVisibleCore(value As Boolean)
        If Not IsHandleCreated Then
            CreateHandle()
            value = False
        End If
        MyBase.SetVisibleCore(value)
    End Sub

    Private Sub BetterFullscreen_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If WindowState = FormWindowState.Normal Then
            'Shown
            UpdateSelectedGame()
            ToggleWindowState(Me, FormWindowState.Normal)
        ElseIf WindowState = FormWindowState.Minimized Then
            ' Hidden
            ToggleWindowState(Me, FormWindowState.Minimized)
        End If
    End Sub

    Private Sub Button_Save_Click(sender As Object, e As EventArgs) Handles Button_Save.Click
        SaveGameSettings()
    End Sub

    Private Sub Button_Remove_Click(sender As Object, e As EventArgs) Handles Button_Remove.Click
        Dim Game As Profile = GetProfile(ComboBox_Games.SelectedItem, Config)

        If MessageBox.Show("Remove Profile '" & Game.Name & "'?", "Confirm Profile Removal", MessageBoxButtons.OKCancel) = DialogResult.OK Then
            RemoveProfile(Game.Name, Config)
            LogEvent("Removed '" & Game.Name & "' settings")
            ComboBox_Games.Items.RemoveAt(ComboBox_Games.SelectedIndex)
            LoadGameSettings()
        End If
    End Sub

    Private Sub ComboBox_Games_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_Games.SelectedIndexChanged
        LoadGameSettings(ComboBox_Games.SelectedIndex)
    End Sub

    Private Sub CheckBox_startWithWindows_CheckedChanged(sender As Object, e As EventArgs)
        RemoveHandler CheckBox_StartWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged
        If LoadWithWindows() Then
            TaskSchedeuler.ToggleTask()
            CheckBox_StartWithWindows.Checked = False
        Else
            TaskSchedeuler.ToggleTask()
            CheckBox_StartWithWindows.Checked = True
        End If
        LogEvent("Toggled start with windows")
        AddHandler CheckBox_StartWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged
    End Sub

    Private Sub Button_SaveApp_Click(sender As Object, e As EventArgs) Handles Button_SaveApp.Click
        SaveSettings()
    End Sub

    Private Sub TrayIcon_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TrayIcon.MouseDoubleClick
        ToggleWindowState(Me)
    End Sub

    Private Sub ToggleWindowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToggleWindowToolStripMenuItem.Click
        ToggleWindowState(Me)
    End Sub

    Private Sub ReloadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReloadToolStripMenuItem.Click
        Process.Start(New ProcessStartInfo With {
            .FileName = "cmd",
            .Arguments = "/C timeout 1 && """ & Application.ExecutablePath & """",
            .UseShellExecute = True,
            .WindowStyle = ProcessWindowStyle.Hidden,
            .CreateNoWindow = True
        })
        CloseSilently = True
        Application.Exit()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        CloseSilently = True
        Application.Exit()
    End Sub

    Private Sub BetterFullscreen_Closing(sender As Object, e As FormClosingEventArgs) Handles Me.Closing
        If e.CloseReason = CloseReason.UserClosing And Not CloseSilently = True Then
            If MessageBox.Show("You are about to exit Better Fullscreen" & vbCrLf & "Press OK to confirm and exit or Cancel to abort", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.Cancel Then
                e.Cancel = True
                Exit Sub
            End If
        End If

        Hotkeys.Dispose()
        UnhookWinEvent(WinEventHook)
    End Sub

    Private Sub ComboBox_Games_Enter(sender As Object, e As EventArgs) Handles ComboBox_Games.Enter
        If ModifierKeys <> Keys.Shift Then Exit Sub
        ComboBox_Games.Enabled = False
        ComboBox_Games.Enabled = True

        ToggleEditProfile()
    End Sub

    Private Sub ComboBox_Games_KeyDown(sender As Object, e As KeyEventArgs) Handles ComboBox_Games.KeyDown
        If e.KeyCode <> Keys.Enter Then Exit Sub
        ToggleEditProfile()
    End Sub

#End Region

    Private Sub LoadGameSettings(Optional SelectedGame As String = Nothing)
        If SelectedGame Is Nothing And Not ComboBox_Games.SelectedIndex = 0 Then
            If ComboBox_Games.Items.Count > 0 Then
                ComboBox_Games.SelectedIndex = 0
                GroupBox_GameSettings.Enabled = True
            Else
                GroupBox_GameSettings.Enabled = False
            End If
        Else
            ComboBox_Games.SelectedIndex = SelectedGame
        End If

        If (SelectedGame Is Nothing And ComboBox_Games.SelectedIndex > 0) Or (SelectedGame IsNot Nothing And Not ComboBox_Games.SelectedItem = SelectedGame) Then
            Dim Game As Profile = GetProfile(ComboBox_Games.SelectedItem, Config)
            If Game Is Nothing Then Exit Sub
            TextBox_Title.Text = Game.Title.Text
            SetRadioButton(Panel_titleRadioButtonContainer, Game.Title.Match)
            TextBox_Class.Text = Game.Class
            NumericUpDown_Width.Value = Game.Size.Width
            NumericUpDown_Height.Value = Game.Size.Height
            NumericUpDown_Top.Value = Game.Location.Y
            NumericUpDown_Left.Value = Game.Location.X
            NumericUpDown_Delay.Value = Game.Delay
            CheckBox_CaptureMouse.Checked = Game.CaptureMouse
            CheckBox_ForceTopMost.Checked = Game.ForceTopMost
            CheckBox_ProfileEnabled.Checked = Game.Enabled
            LogEvent("Loaded '" & Game.Name & "' settings")
        End If
    End Sub

    Private Sub SaveGameSettings()
        If ComboBox_Games.SelectedIndex > -1 Then
            Dim Game As Profile = GetProfile(ComboBox_Games.SelectedItem, Config)
            Game.Title.Text = TextBox_Title.Text
            Game.Title.Match = CType(GetRadioButton(Panel_titleRadioButtonContainer).Tag, MatchType)
            Game.Class = TextBox_Class.Text
            Game.Size = New Size(NumericUpDown_Width.Value, NumericUpDown_Height.Value)
            Game.Location = New Point(NumericUpDown_Left.Value, NumericUpDown_Top.Value)
            Game.Delay = NumericUpDown_Delay.Value
            Game.CaptureMouse = CheckBox_CaptureMouse.Checked
            Game.Enabled = CheckBox_ProfileEnabled.Checked
            Game.ForceTopMost = CheckBox_ForceTopMost.Checked
            SaveConfig(ConfigPath, Config)
            LogEvent("Saved '" & Game.Name & "' settings")
        End If
    End Sub

    Public Function LoadWithWindows() As Boolean
        Return TaskSchedeuler.GetTask().Enabled
    End Function

    Private Sub LoadSettings()
        ComboBox_Key.SelectedIndex = Config.Settings.Hotkey - 112
        ComboBox_Modifier.SelectedIndex = Config.Settings.Modifier
        NumericUpDown_DefaultWidth.Value = Config.Settings.DefaultSize.Width
        NumericUpDown_DefaultHeight.Value = Config.Settings.DefaultSize.Height
        NumericUpDown_DefaultLeft.Value = Config.Settings.DefaultLocation.X
        NumericUpDown_DefaultTop.Value = Config.Settings.DefaultLocation.Y
        ComboBox_TriggerEvents.SelectedIndex = Config.Settings.TriggerEvents
        CheckBox_StartHidden.Checked = Config.Settings.StartHidden
        LogEvent("Loaded application settings")
    End Sub

    Private Sub SaveSettings()
        Config.Settings.Hotkey = CType(ComboBox_Key.SelectedIndex + 112, Keys)
        Config.Settings.Modifier = CType(ComboBox_Modifier.SelectedIndex, ModifierKey)
        Config.Settings.DefaultSize = New Size(NumericUpDown_DefaultWidth.Value, NumericUpDown_DefaultHeight.Value)
        Config.Settings.DefaultLocation = New Point(NumericUpDown_DefaultLeft.Value, NumericUpDown_DefaultTop.Value)
        Config.Settings.TriggerEvents = CType(ComboBox_TriggerEvents.SelectedIndex, TriggerEvent)
        Config.Settings.StartHidden = CheckBox_StartHidden.Checked
        SaveConfig(ConfigPath, Config)
        LogEvent("Saved application settings")
    End Sub

    Private Sub ToggleEditProfile()
        If ComboBox_Games.DropDownStyle = ComboBoxStyle.Simple Then
            GetProfile(ComboBox_Games.Items.Item(ComboBox_Games.Tag.Split(","c)(0)), Config).Name = ComboBox_Games.Text
            ComboBox_Games.Items.Add(ComboBox_Games.Text)
            ComboBox_Games.Items.Remove(ComboBox_Games.Tag.Split(","c)(1))
            ComboBox_Games.SelectedItem = ComboBox_Games.Text
            ComboBox_Games.Tag = Nothing
            ComboBox_Games.DropDownStyle = ComboBoxStyle.DropDownList
        Else
            ComboBox_Games.Tag = ComboBox_Games.SelectedIndex & "," & ComboBox_Games.SelectedItem
            ComboBox_Games.DropDownStyle = ComboBoxStyle.Simple
        End If
    End Sub

    Private Sub Init()
        LogEvent("Using Windows DPI scale factor of " & WindowsScaleFactor)
        ComboBox_Games.Items.AddRange(GetProfileNames(Config).ToArray())
        LogEvent("Loaded all games")
        LoadSettings()
        LoadGameSettings()
        UpdateSelectedGame()
        LogEvent("Ready")
    End Sub

    Private Sub LogEvent(msg As String)
        Dim LogMsg = "[" & Now.ToString("HH:mm:ss") & "] " & msg

        If Not RichTextBox_EventLog.IsDisposed Then
            RichTextBox_EventLog.AppendText(LogMsg & vbCrLf)
        End If

        Debug.WriteLine("[" & Now.ToString("HH:mm:ss") & "] " & msg)
    End Sub

    Private Sub DoWork(windowTitle As String, windowClass As String, Window_HWND As IntPtr)
        Debug.WriteLine("Active Window Title: " & windowTitle & ", Active Window Class: " & windowClass)

        Dim CurrentGame As Profile = GetCurrentProfile(Config)
        Dim Game As Profile = FindProfile(windowTitle, windowClass, Config)

        If CurrentGame Is Nothing And Game Is Nothing Then Exit Sub

        ' Game Started
        If Game IsNot Nothing And CurrentGame Is Nothing Then
            If Not Game.State = GameState.None Then Exit Sub

            LogEvent(Game.Name & " started")

            Game.IsCurrentProfile = True
            Game.UnsafeTitle = GetWindowTitle(Window_HWND, False)
            Game.UnsafeClass = GetWindowClass(Window_HWND, False)
            Game.State = GameState.Started
            CurrentGame = Game

            UpdateSelectedGame()

            If Game.Delay > 0 Then
                LogEvent("delaying actions for " & Game.Delay & "ms")
                Thread.Sleep(Game.Delay)
            End If

            LogEvent("setting WS_VISIBLE")
            SetWindowLong(Window_HWND, GWL.STYLE, WS.VISIBLE)
            SetWindowPos(Window_HWND, HWND.TOP, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE Or SWP.FRAMECHANGED)
        End If

        ' Window Has Focus
        If Game IsNot Nothing And CurrentGame IsNot Nothing Then
            Dim scaledWidth As Integer = Game.Size.Width / WindowsScaleFactor
            Dim scaledHeight As Integer = Game.Size.Height / WindowsScaleFactor
            Dim windowRect = GetWindowRectangle(Window_HWND)
            Dim correctPos = New Point(windowRect.X, windowRect.Y) = Game.Location
            Dim correctSize = New Size(windowRect.Width, windowRect.Height) = New Size(scaledWidth, scaledHeight)

            If Not correctSize Then
                LogEvent("resizing window " & Game.Size.ToString())
                SetWindowPos(Window_HWND, HWND.TOP, 0, 0, scaledWidth, scaledHeight, SWP.NOMOVE)
            End If
            If Not correctPos Then
                LogEvent("repositioning window " & Game.Location.ToString())
                SetWindowPos(Window_HWND, HWND.TOP, Game.Location.X, Game.Location.Y, 0, 0, SWP.NOSIZE)
            End If
            If Game.ForceTopMost And Not IsWindowTopMost(Window_HWND) Then
                LogEvent("setting HWND_TOPMOST")
                SetWindowPos(Window_HWND, HWND.TOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE)
            End If
            If Game.CaptureMouse And Not IsCursorClipped() Then
                LogEvent("locking mouse to game window location")
                Cursor.Clip = New Rectangle(Game.Location, Game.Size)
            End If
            If Game.State = GameState.Focused Then Exit Sub

            Game.State = GameState.Focused
            LogEvent(Game.Name & " has focus")
        End If

        ' Window Unfocused / Closed
        If Game Is Nothing And CurrentGame IsNot Nothing Then

            Dim Game_HWND = FindWindow(CurrentGame.UnsafeClass, CurrentGame.UnsafeTitle)
            Dim CurrentWindow_HWND = GetForegroundWindow()

            If Game_HWND <> IntPtr.Zero Then ' Window Unfocused
                If CurrentGame.ForceTopMost And IsWindowTopMost(Game_HWND) Then
                    LogEvent("setting HWND_NOTOPMOST")
                    SetWindowPos(Game_HWND, HWND.NOTOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE Or SWP.NOACTIVATE)
                End If
                If CurrentGame.CaptureMouse And IsCursorClipped() Then
                    LogEvent("releasing mouse lock from game window location")
                    Cursor.Clip = New Rectangle()
                End If
                If CurrentGame.State = GameState.Unfocused Then Exit Sub

                CurrentGame.State = GameState.Unfocused
                LogEvent(CurrentGame.Name & " lost focus")
            Else ' Window Closed
                If CurrentGame.CaptureMouse And IsCursorClipped() Then
                    LogEvent("releasing mouse lock from game window location")
                    Cursor.Clip = New Rectangle()
                End If

                CurrentGame.IsCurrentProfile = False
                CurrentGame.State = GameState.None
                LogEvent(CurrentGame.Name & " exited")
            End If
        End If
    End Sub

    Private Sub AddGame()
        Dim HWND = GetForegroundWindow()
        Dim NewGame As New Profile With {
            .Title = New Title With {
                .Text = GetWindowTitle(HWND),
                .Match = MatchType.Full
            },
            .Class = GetWindowClass(HWND),
            .Size = Config.Settings.DefaultSize,
            .Location = Config.Settings.DefaultLocation,
            .Name = .Title.Text.Replace("[", "(").Replace("]", ")")
        }

        AddProfile(NewGame, Config)
        ComboBox_Games.Items.Add(NewGame.Name)
        UpdateSelectedGame()

        LogEvent(NewGame.Name & " added")
        LogEvent("size " & NewGame.Size.ToString)
        LogEvent("location " & NewGame.Location.ToString())

        DoWork(NewGame.Title.Text, NewGame.[Class], HWND)
    End Sub

    Private Sub UpdateSelectedGame()
        If ComboBox_Games.Items.Count = 0 Then Exit Sub

        Dim CurrentProfile = GetCurrentProfile(Config)

        If ComboBox_Games.SelectedIndex > -1 And CurrentProfile IsNot Nothing Then
            ComboBox_Games.SelectedItem = CurrentProfile.Name
        Else
            ComboBox_Games.SelectedIndex = 0
        End If
    End Sub

    Private Sub WinEventProc(hWinEventHook As IntPtr, eventType As UInteger, HWND As IntPtr, idObject As Integer, idChild As Integer, dwEventThread As UInteger, dwmsEventTime As UInteger)
        If Now.Subtract(DebounceTrigger.Key).TotalSeconds < 1.0 And DebounceTrigger.Value = HWND Then Exit Sub

        DebounceTrigger = New KeyValuePair(Of Date, IntPtr)(Now, HWND)
        Dim TriggerEvents = Config.Settings.TriggerEvents

        If (TriggerEvents = TriggerEvent.ForegroundWindowChanged Or TriggerEvents = TriggerEvent.Both) And eventType = WIN_EVENT.EVENT_SYSTEM_FOREGROUND Then
            Debug.WriteLine("Event: EVENT_SYSTEM_FOREGROUND")
            DoWork(GetWindowTitle(HWND), GetWindowClass(HWND), HWND)
            Exit Sub
        ElseIf (TriggerEvents = TriggerEvent.MouseCaptureStart Or TriggerEvents = TriggerEvent.Both) And eventType = WIN_EVENT.EVENT_SYSTEM_CAPTURESTART Then
            Debug.WriteLine("Event: EVENT_SYSTEM_CAPTURESTART")
            DoWork(GetWindowTitle(HWND), GetWindowClass(HWND), HWND)
            Exit Sub
        End If
    End Sub

End Class