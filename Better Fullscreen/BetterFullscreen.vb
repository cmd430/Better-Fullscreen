Imports System.ComponentModel
Imports System.Threading
Imports Microsoft.Win32
Imports Microsoft.Win32.Registry

Public Class BetterFullscreen

#Region "Form Events"
    Public Delegate Sub ActiveWindowChangedHandler(sender As Object, windowTitle As String, windowClass As String, HWND As IntPtr)

    Private WindowsScaleFactor As Int32 = 1

    Private __hWinHook As IntPtr
    Private __winEventProc As WinEventDelegate

    Private ReadOnly __Hotkeys As New Hotkeys
    Private ReadOnly __TaskSchedeuler As New Scheduler("Better Fullscreen", "cmd430", "Starts Better Fullscreen on Logon", Nothing, Nothing, False)

    Private ReadOnly ConfigPath As String = Application.ExecutablePath.Replace(".exe", ".conf")
    Private Config As BetterFullscreenConfig = LoadConfig(ConfigPath)


    Private Sub BetterFullscreen_Load(sender As Object, e As EventArgs) Handles Me.Load
        __winEventProc = New WinEventDelegate(AddressOf WinEventProc)
        __hWinHook = SetWinEventHook(WIN_EVENT.EVENT_SYSTEM_FOREGROUND, WIN_EVENT.EVENT_SYSTEM_CAPTURESTART, IntPtr.Zero, __winEventProc, 0, 0, WIN_EVENT_FLAGS.WINEVENT_OUTOFCONTEXT)

        AddHandler __Hotkeys.KeyPressed, AddressOf AddGame
        __Hotkeys.RegisterHotKey(Config.Settings.Modifier, Config.Settings.Hotkey)

        If __TaskSchedeuler.GetTask() Is Nothing Then
            __TaskSchedeuler.AddTask()
        Else
            __TaskSchedeuler.UpdateTask()
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

        If Debugger.IsAttached Or Not Config.Settings.StartHidden Then
            WindowState = FormWindowState.Normal
        Else
            WindowState = FormWindowState.Minimized
        End If

        Show()
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
            LoadSettings()
            LoadGameSettings()
            UpdateSelectedGame()
            ShowInTaskbar = True
        ElseIf WindowState = FormWindowState.Minimized Then
            ' Hidden
            ShowInTaskbar = False
        End If
        SendMessage(Handle, WIN_MESSAGE.WM_SETICON, ICON_SIZE.ICON_SMALL, My.Resources.Fullscreen_Dark.Handle)
    End Sub

    Private Sub Button_Save_Click(sender As Object, e As EventArgs) Handles Button_Save.Click
        SaveGameSettings()
    End Sub

    Private Sub Button_Remove_Click(sender As Object, e As EventArgs) Handles Button_Remove.Click
        Dim Game As Profile = GetProfile(ComboBox_Games.SelectedItem, Config)
        Dim confirm As Integer = MessageBox.Show("Remove Profile '" & Game.Name & "'?", "Confirm Profile Removal", MessageBoxButtons.OKCancel)

        If confirm = DialogResult.OK Then
            RemoveProfile(Game.Name, Config)
            LogEvent("Removed '" & Game.Name & "' settings")
            ComboBox_Games.Items.RemoveAt(ComboBox_Games.SelectedIndex)
            LoadGameSettings()
        End If
    End Sub

    Private Sub ComboBox_Games_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_Games.SelectedIndexChanged
        LoadGameSettings(ComboBox_Games.SelectedIndex)
    End Sub

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
        Return __TaskSchedeuler.GetTask().Enabled
    End Function

    Private Sub CheckBox_startWithWindows_CheckedChanged(sender As Object, e As EventArgs)
        RemoveHandler CheckBox_StartWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged
        If LoadWithWindows() Then
            __TaskSchedeuler.ToggleTask()
            CheckBox_StartWithWindows.Checked = False
        Else
            __TaskSchedeuler.ToggleTask()
            CheckBox_StartWithWindows.Checked = True
        End If
        LogEvent("Toggled start with windows")
        AddHandler CheckBox_StartWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged
    End Sub

    Private Sub Button_SaveApp_Click(sender As Object, e As EventArgs) Handles Button_SaveApp.Click
        SaveSettings()
    End Sub

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

    Private Sub TrayIcon_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TrayIcon.MouseDoubleClick
        ToggleWindowToolStripMenuItem.PerformClick()
    End Sub

    Private Sub ToggleWindowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToggleWindowToolStripMenuItem.Click
        If WindowState = FormWindowState.Normal Then
            WindowState = FormWindowState.Minimized
        Else
            WindowState = FormWindowState.Normal
        End If
    End Sub

    Private Sub ReloadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReloadToolStripMenuItem.Click
        Application.Restart()
        Application.ExitThread()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub BetterFullscreen_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Visible And Not Debugger.IsAttached Then
            e.Cancel = True
            ToggleWindowToolStripMenuItem.PerformClick()
        Else
            __Hotkeys.Dispose()
            UnhookWinEvent(__hWinHook)
        End If
    End Sub


    ' Allow editing profile name without editing .conf file
    Private Sub ComboBox_Games_Enter(sender As Object, e As EventArgs) Handles ComboBox_Games.Enter
        If Control.ModifierKeys <> Keys.Shift Then Exit Sub
        ComboBox_Games.Enabled = False
        ComboBox_Games.Enabled = True

        ToggleEditProfile()
    End Sub

    Private Sub ComboBox_Games_KeyDown(sender As Object, e As KeyEventArgs) Handles ComboBox_Games.KeyDown
        If e.KeyCode <> Keys.Enter Then Exit Sub
        ToggleEditProfile()
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

#End Region

    Private Sub Init()
        Using Key As RegistryKey = CurrentUser.OpenSubKey("Control Panel\Desktop\WindowMetrics")
            If Key IsNot Nothing Then
                Dim ADPI As Object = Key.GetValue("AppliedDPI")
                If ADPI IsNot Nothing Then
                    WindowsScaleFactor = Convert.ToInt32(ADPI) / 96
                End If
            End If
        End Using

        LogEvent("Using Windows DPI scale factor of " & WindowsScaleFactor)
        LogEvent("Loading Games")

        ComboBox_Games.Items.AddRange(GetProfileNames(Config).ToArray())

        LogEvent("Loaded all games")
        LogEvent("Ready")

        LoadSettings()
        LoadGameSettings()
        UpdateSelectedGame()
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

            If Game.Delay > 0 Then
                LogEvent("delaying actions for " & Game.Delay & "ms")
                Thread.Sleep(Game.Delay)
            End If

            LogEvent("setting WS_VISIBLE")
            SetWindowLong(Window_HWND, GWL.STYLE, WS.VISIBLE)
            LogEvent("resizing window " & Game.Size.ToString())
            LogEvent("repositioning window " & Game.Location.ToString())
            SetWindowPos(Window_HWND, HWND.TOP, Game.Location.X, Game.Location.Y, Game.Size.Width / WindowsScaleFactor, Game.Size.Height / WindowsScaleFactor, SWP.FRAMECHANGED)

            If Game.CaptureMouse Then
                Cursor.Clip = New Rectangle(Game.Location, Game.Size)
            End If

            Game.IsCurrentProfile = True
            Game.UnsafeTitle = GetWindowTitle(Window_HWND, False)
            Game.UnsafeClass = GetWindowClass(Window_HWND, False)
            Game.State = GameState.Started
            CurrentGame = Game
        End If

        ' Window Has Focus
        If Game IsNot Nothing And CurrentGame IsNot Nothing Then
            If Not Game.State = GameState.Started And Not Game.State = GameState.Unfocused Then Exit Sub

            LogEvent(Game.Name & " has focus")

            If Game.ForceTopMost And Not GetWindowLong(Window_HWND, GWL.EXSTYLE) = 262152 Then
                LogEvent("setting HWND_TOPMOST")
                SetWindowPos(Window_HWND, HWND.TOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE Or SWP.NOACTIVATE)
            End If
            If Game.CaptureMouse Then
                LogEvent("locking mouse to game window location")
                Cursor.Clip = New Rectangle(Game.Location, Game.Size)
            End If

            Dim rect As New RECT
            Dim Window_Rect = New Rectangle()

            GetWindowRect(Window_HWND, rect)

            Window_Rect.X = rect.left
            Window_Rect.Y = rect.top
            Window_Rect.Width = rect.right - rect.left
            Window_Rect.Height = rect.bottom - rect.top

            Dim correctPos = New Point(Window_Rect.X, Window_Rect.Y) = Game.Location
            Dim correctSize = New Size(Window_Rect.Width, Window_Rect.Height) = New Size(Game.Size.Width / WindowsScaleFactor, Game.Size.Height / WindowsScaleFactor)

            If Not correctPos Or Not correctSize Then
                LogEvent("resizing window " & Game.Size.ToString())
                LogEvent("repositioning window " & Game.Location.ToString())
                SetWindowPos(Window_HWND, HWND.TOP, Game.Location.X, Game.Location.Y, Game.Size.Width / WindowsScaleFactor, Game.Size.Height / WindowsScaleFactor, SWP.FRAMECHANGED)
            End If

            Game.State = GameState.Focused
        End If

        ' Window Unfocused / Closed
        If Game Is Nothing And CurrentGame IsNot Nothing Then

            Dim Game_HWND = FindWindowW(CurrentGame.UnsafeClass, CurrentGame.UnsafeTitle)
            Dim CurrentWindow_HWND = GetForegroundWindow()

            If Game_HWND <> IntPtr.Zero Then ' Window Unfocused
                If Not CurrentGame.State = GameState.Focused Then Exit Sub

                LogEvent(CurrentGame.Name & " lost focus")

                If CurrentGame.ForceTopMost Then
                    LogEvent("setting HWND_NOTOPMOST")
                    SetWindowPos(Game_HWND, HWND.NOTOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE Or SWP.NOACTIVATE)
                End If
                If CurrentGame.CaptureMouse Then
                    LogEvent("releasing mouse lock from game window location")
                    Cursor.Clip = New Rectangle(New Point(0, 0), SystemInformation.VirtualScreen.Size)
                End If

                CurrentGame.State = GameState.Unfocused
            Else ' Window Closed
                LogEvent(CurrentGame.Name & " exited")

                If CurrentGame.CaptureMouse Then
                    LogEvent("releasing mouse lock from game window location")
                    Cursor.Clip = New Rectangle(New Point(0, 0), SystemInformation.VirtualScreen.Size)
                End If

                CurrentGame.IsCurrentProfile = False
                CurrentGame.State = GameState.None
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