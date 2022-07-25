﻿Imports System.ComponentModel
Imports System.Threading
Imports Microsoft.Win32.Registry

Public Class BetterFullscreen

#Region "Form Events"
    Public Delegate Sub ActiveWindowChangedHandler(sender As Object, windowTitle As String, windowClass As String, HWND As IntPtr)

    Private WindowsScaleFactor As Int32 = 1

    Private __SettingsChanged As Boolean = False
    Private __hWinHook As IntPtr
    Private __winEventProc As WinEventDelegate

    Private ReadOnly __Hotkeys As New Hotkeys
    Private ReadOnly __TaskSchedeuler As New Scheduler("Better Fullscreen", "cmd430", "Starts Better Fullscreen on Logon", Nothing, Nothing, False)

    Private ReadOnly ConfigPath As String = Application.ExecutablePath.Replace(".exe", ".conf")
    Private Config As BetterFullscreenConfig = LoadConfig(ConfigPath)


    Private Sub BetterFullscreen_Load(sender As Object, e As EventArgs) Handles Me.Load
        __winEventProc = New WinEventDelegate(AddressOf WinEventProc)
        __hWinHook = SetWinEventHook(WIN_EVENT.EVENT_SYSTEM_FOREGROUND, WIN_EVENT.EVENT_SYSTEM_CAPTURESTART, IntPtr.Zero, __winEventProc, 0, 0, WIN_EVENT_FLAGS.WINEVENT_OUTOFCONTEXT)

        AddHandler __Hotkeys.KeyPressed, AddressOf Add_Game
        __Hotkeys.RegisterHotKey(Config.Settings.Modifier, Config.Settings.Hotkey)

        If __TaskSchedeuler.GetTask() Is Nothing Then
            __TaskSchedeuler.AddTask()
        Else
            __TaskSchedeuler.UpdateTask()
        End If

        If LoadWithWindows() Then
            CheckBox_startWithWindows.Checked = True
        End If

        AddHandler CheckBox_startWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged

        Init()
    End Sub

    Protected Overrides Sub SetVisibleCore(value As Boolean)
        If Not IsHandleCreated Then
            CreateHandle()
            value = Debugger.IsAttached ' Start hidden (unless debugging)
        End If
        MyBase.SetVisibleCore(value)
    End Sub

    Private Sub BetterFullscreen_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Visible Then
            LoadSettings()
            LoadGameSettings()

            Dim CurrentProfile = GetCurrentProfile(Config)

            If ComboBox_Games.SelectedIndex > -1 And GetCurrentProfile(Config) IsNot Nothing Then
                ComboBox_Games.SelectedItem = CurrentProfile.Name
            End If
        End If
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
            TextBox_Title.Text = Game.Title
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
            Game.Title = TextBox_Title.Text
            Game.Class = TextBox_Class.Text
            Game.Size = New Size(NumericUpDown_Width.Value, NumericUpDown_Height.Value)
            Game.Location = New Point(NumericUpDown_Left.Value, NumericUpDown_Top.Value)
            Game.Delay = NumericUpDown_Delay.Value
            Game.CaptureMouse = CheckBox_CaptureMouse.Checked
            Game.Enabled = CheckBox_ProfileEnabled.Checked
            Game.ForceTopMost = CheckBox_ForceTopMost.Checked
            SaveConfig(ConfigPath, Config)
            LogEvent("Saved '" & Game.Name & "' settings")
            __SettingsChanged = True
        End If
    End Sub

    Public Function LoadWithWindows() As Boolean
        Return __TaskSchedeuler.GetTask().Enabled
    End Function

    Private Sub CheckBox_startWithWindows_CheckedChanged(sender As Object, e As EventArgs)
        RemoveHandler CheckBox_startWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged
        If LoadWithWindows() Then
            __TaskSchedeuler.ToggleTask()
            CheckBox_startWithWindows.Checked = False
        Else
            __TaskSchedeuler.ToggleTask()
            CheckBox_startWithWindows.Checked = True
        End If
        LogEvent("Toggled start with windows")
        AddHandler CheckBox_startWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged
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
        LogEvent("Loaded application settings")
    End Sub

    Private Sub SaveSettings()
        Config.Settings.Hotkey = CType(ComboBox_Key.SelectedIndex + 112, Keys)
        Config.Settings.Modifier = CType(ComboBox_Modifier.SelectedIndex, ModifierKey)
        Config.Settings.DefaultSize = New Size(NumericUpDown_DefaultWidth.Value, NumericUpDown_DefaultHeight.Value)
        Config.Settings.DefaultLocation = New Point(NumericUpDown_DefaultLeft.Value, NumericUpDown_DefaultTop.Value)
        SaveConfig(ConfigPath, Config)
        LogEvent("Saved application settings")
        __SettingsChanged = True
    End Sub

    Private Sub TrayIcon_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TrayIcon.MouseDoubleClick
        ToggleWindowToolStripMenuItem.PerformClick()
    End Sub

    Private Sub ToggleWindowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToggleWindowToolStripMenuItem.Click
        If Visible Then
            Hide()
            If __SettingsChanged Then
                Application.Restart()
            End If
        Else
            Show()
            Focus()
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
        If Visible Then
            e.Cancel = True
            ToggleWindowToolStripMenuItem.PerformClick()
        Else
            __Hotkeys.Dispose()
            UnhookWinEvent(__hWinHook)
        End If
    End Sub

#End Region

    Private Sub Init()
        Using Key = CurrentUser.OpenSubKey("Control Panel\Desktop\WindowMetrics")
            If Key IsNot Nothing Then
                Dim ADPI = Key.GetValue("AppliedDPI")
                If ADPI IsNot Nothing Then
                    WindowsScaleFactor = Convert.ToInt32(ADPI) / 96
                End If
            End If
        End Using
        LogEvent("Using Windows DPI scale factor of " & WindowsScaleFactor)
        LogEvent("Loading Games")
        For Each profile As String In GetProfileNames(Config)
            ComboBox_Games.Items.Add(profile)
        Next
        LogEvent("Loaded all games")
        LogEvent("Ready")

        LoadSettings()
        LoadGameSettings()

        Dim CurrentProfile = GetCurrentProfile(Config)

        If ComboBox_Games.SelectedIndex > -1 And GetCurrentProfile(Config) IsNot Nothing Then
            ComboBox_Games.SelectedItem = CurrentProfile.Name
        End If
    End Sub

    Private Sub LogEvent(msg As String)
        Dim LogMsg = "[" & Now.ToString("HH:mm:ss") & "] " & msg

        RichTextBox_EventLog.AppendText(LogMsg & vbCrLf)
        Debug.WriteLine("[" & Now.ToString("HH:mm:ss") & "] " & msg)
    End Sub

    Private Sub DoWork(windowTitle As String, windowClass As String, Window_HWND As IntPtr)
        Debug.WriteLine("Active Window Title: " & windowTitle & ", Active Window Class: " & windowClass)

        For Each Game As Profile In GetProfiles(Config)
            If Game.Enabled And ((Not Game.Class = "" And Game.Class = windowClass) Or Game.Class = "") And ((Not Game.Title = "" And Game.Title = windowTitle) Or Game.Title = "") Then
                If Game.State = GameState.None Then
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
                    Game.State = GameState.Focused
                    Game.IsCurrentProfile = True
                    Game.UnsafeTitle = GetWindowTitle(Window_HWND, False)
                    Game.UnsafeClass = GetWindowClass(Window_HWND, False)
                End If
                If Game.State <> GameState.None Then
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
                End If
                If Game.State = GameState.Focused Then
                    LogEvent(Game.Name & " has focus")
                    If Game.ForceTopMost And Not GetWindowLong(Window_HWND, GWL.EXSTYLE) = 262152 Then
                        LogEvent("setting HWND_TOPMOST")
                        SetWindowPos(Window_HWND, HWND.TOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE Or SWP.NOACTIVATE)
                    End If
                    If Game.CaptureMouse Then
                        LogEvent("locking mouse to game window location")
                        Cursor.Clip = New Rectangle(Game.Location, Game.Size)
                    End If
                    Game.State = GameState.Unfocused
                End If
            Else
                Dim CurrentGame = GetCurrentProfile(Config)
                If CurrentGame IsNot Nothing Then
                    Dim Game_HWND = FindWindowW(CurrentGame.UnsafeClass, CurrentGame.UnsafeTitle)
                    Dim CurrentWindow_HWND = GetForegroundWindow()

                    If Game_HWND <> IntPtr.Zero Then
                        If Game.State = GameState.Unfocused Then
                            LogEvent(Game.Name & " lost focus")
                            If Game.ForceTopMost Then
                                LogEvent("setting HWND_NOTOPMOST")
                                SetWindowPos(Game_HWND, HWND.NOTOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE Or SWP.NOACTIVATE)

                                ' Fix not bringing clicked on windows to foreground
                                If CurrentWindow_HWND <> IntPtr.Zero Then
                                    Dim blackListTitles As New List(Of String)({"Task Switching", "Task View", "Start", "Search"})
                                    Dim blackListClasses As New List(Of String)({"Shell_TrayWnd", "WindowsDashboard"})

                                    If Not blackListTitles.Any(Function(s) GetWindowTitle(CurrentWindow_HWND).Contains(s)) And Not blackListClasses.Any(Function(s) GetWindowClass(CurrentWindow_HWND).Contains(s)) Then
                                        SetForegroundWindow(FindWindowW("Shell_TrayWnd", Nothing))
                                        SetForegroundWindow(CurrentWindow_HWND)
                                    End If
                                End If
                            End If
                            If Game.CaptureMouse Then
                                LogEvent("releasing mouse lock from game window location")
                                Cursor.Clip = New Rectangle(New Point(0, 0), SystemInformation.VirtualScreen.Size)
                            End If
                            Game.State = GameState.Focused
                        End If
                    Else
                        If Game.State <> GameState.None Then
                            LogEvent(Game.Name & " exited")
                            If Game.CaptureMouse Then
                                LogEvent("releasing mouse lock from game window location")
                                Cursor.Clip = New Rectangle(New Point(0, 0), SystemInformation.VirtualScreen.Size)
                            End If
                            Game.State = GameState.None
                            Game.IsCurrentProfile = False
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub Add_Game()
        Dim HWND = GetForegroundWindow()
        Dim NewGame As New Profile With {
            .Title = GetWindowTitle(HWND),
            .[Class] = GetWindowClass(HWND),
            .Size = Config.Settings.DefaultSize,
            .Location = Config.Settings.DefaultLocation,
            .Name = .Title.Replace("[", "(").Replace("]", ")")
        }

        If NewGame.Title = "" Or GetProfileNames(Config).Contains(NewGame.Name) Then Exit Sub

        AddProfile(NewGame, Config)
        ComboBox_Games.Items.Add(NewGame.Name)
        ComboBox_Games.SelectedItem = NewGame.Name

        LogEvent(NewGame.Name & " added")
        LogEvent("size " & NewGame.Size.ToString)
        LogEvent("location " & NewGame.Location.ToString())

        DoWork(NewGame.Title, NewGame.[Class], HWND)
    End Sub

    Private Sub WinEventProc(hWinEventHook As IntPtr, eventType As UInteger, HWND As IntPtr, idObject As Integer, idChild As Integer, dwEventThread As UInteger, dwmsEventTime As UInteger)
        If eventType = WIN_EVENT.EVENT_SYSTEM_FOREGROUND Or eventType = WIN_EVENT.EVENT_SYSTEM_CAPTURESTART Then
            DoWork(GetWindowTitle(HWND), GetWindowClass(HWND), HWND)
        End If
    End Sub

End Class