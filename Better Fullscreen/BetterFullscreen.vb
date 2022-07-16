Imports System.ComponentModel
Imports System.DateTime
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32.Registry

Public Class BetterFullscreen

#Region "Native Methods"
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="FindWindowW")>
    Public Shared Function FindWindowW(<MarshalAs(UnmanagedType.LPTStr)> ByVal lpClassName As String, <MarshalAs(UnmanagedType.LPTStr)> ByVal lpWindowName As String) As IntPtr
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetForegroundWindow")>
    Private Shared Function GetForegroundWindow() As IntPtr
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="SetWindowLong")>
    Private Shared Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As IntPtr) As Integer
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetWindowLong")>
    Private Shared Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Integer
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="SetWindowPos")>
    Private Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As Integer) As Boolean
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetWindowText", CallingConvention:=CallingConvention.StdCall)>
    Private Shared Function GetWindowText(ByVal hWnd As Integer, lpString As StringBuilder, count As Integer) As Integer
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetClassName")>
    Public Shared Function GetClassName(ByVal hWnd As IntPtr, ByVal lpClassName As StringBuilder, ByVal nMaxCount As Integer) As Integer
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetWindowRect")>
    Private Shared Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, CallingConvention:=CallingConvention.StdCall)>
    Private Shared Function SetWinEventHook(ByVal eventMin As UInteger, ByVal eventMax As UInteger, ByVal hmodWinEventProc As IntPtr, ByVal lpfnWinEventProc As WinEventDelegate, ByVal idProcess As UInteger, ByVal idThread As UInteger, ByVal dwFlags As UInteger) As IntPtr
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, CallingConvention:=CallingConvention.StdCall)>
    Private Shared Function UnhookWinEvent(ByVal hWinEventHook As IntPtr) As Boolean
    End Function
#End Region

#Region "Win32 Enums & Structures"
    <StructLayout(LayoutKind.Sequential)>
    Private Structure RECT
        Public left, top, right, bottom As Integer
    End Structure
    <FlagsAttribute>
    Public Enum ModifierKey As Long
        None = 0
        Alt = 1
        Control = 2
        Shift = 4
        Win = 8
    End Enum
    <FlagsAttribute()>
    Public Enum HWND As Long
        TOP = 0
        BOTTOM = 1
        TOPMOST = -1
        NOTOPMOST = -2
    End Enum
    <FlagsAttribute()>
    Public Enum GWL As Long
        WNDPROC = -4
        HINSTANCE = -6
        HWNDPARENT = -8
        STYLE = -16
        EXSTYLE = -20
        USERDATA = -21
        ID = -12
    End Enum
    <FlagsAttribute()>
    Public Enum WS As Long
        OVERLAPPED = 0
        POPUP = 2147483648
        CHILD = 1073741824
        MINIMIZE = 536870912
        VISIBLE = 268435456
        DISABLED = 134217728
        CLIPSIBLINGS = 67108864
        CLIPCHILDREN = 33554432
        MAXIMIZE = 16777216
        BORDER = 8388608
        DLGFRAME = 4194304
        VSCROLL = 2097152
        HSCROLL = 1048576
        SYSMENU = 524288
        THICKFRAME = 262144
        GROUP = 131072
        TABSTOP = 65536
        MINIMIZEBOX = 131072
        MAXIMIZEBOX = 65536
        CAPTION = BORDER Or DLGFRAME
        TILED = OVERLAPPED
        ICONIC = MINIMIZE
        SIZEBOX = THICKFRAME
        TILEDWINDOW = OVERLAPPEDWINDOW
        OVERLAPPEDWINDOW = OVERLAPPED Or CAPTION Or SYSMENU Or THICKFRAME Or MINIMIZEBOX Or MAXIMIZEBOX
        POPUPWINDOW = POPUP Or BORDER Or SYSMENU
        CHILDWINDOW = CHILD
    End Enum
    <FlagsAttribute()>
    Public Enum WS_EX As Long
        None = 0
        DLGMODALFRAME = 1
        NOPARENTNOTIFY = 4
        TOPMOST = 8
        ACCEPTFILES = 16
        TRANSPARENT = 32
        MDICHILD = 64
        TOOLWINDOW = 128
        WINDOWEDGE = 256
        CLIENTEDGE = 512
        CONTEXTHELP = 1024
        RIGHT = 4096
        LEFT = 0
        RTLREADING = 8192
        LTRREADING = 0
        LEFTSCROLLBAR = 16384
        RIGHTSCROLLBAR = 0
        CONTROLPARENT = 65536
        STATICEDGE = 131072
        APPWINDOW = 262144
        LAYERED = 524288
        NOINHERITLAYOUT = 1048576
        LAYOUTRTL = 4194304
        COMPOSITED = 33554432
        NOACTIVATE = 67108864
        OVERLAPPEDWINDOW = WINDOWEDGE Or CLIENTEDGE
        PALETTEWINDOW = WINDOWEDGE Or TOOLWINDOW Or TOPMOST
    End Enum
    <FlagsAttribute()>
    Public Enum SWP As Long
        ASYNCWINDOWPOS = 4000
        DEFERERASE = 2000
        DRAWFRAME = 20
        FRAMECHANGED = 20
        HIDEWINDOW = 80
        NOACTIVATE = 10
        NOCOPYBITS = 100
        NOMOVE = 2
        NOOWNERZORDER = 200
        NOREDRAW = 8
        NOREPOSITION = 200
        NOSENDCHANGING = 400
        NOSIZE = 1
        NOZORDER = 4
        SHOWWINDOW = 40
    End Enum
    <FlagsAttribute()>
    Public Enum WIN_EVENT As UInteger
        WINEVENT_OUTOFCONTEXT = 0
        ' Im missing some here
    End Enum
    <FlagsAttribute()>
    Public Enum HOOK_EVENT As UInteger
        EVENT_SYSTEM_FOREGROUND = 3
        EVENT_SYSTEM_MINIMIZEEND = 17
        ' Im missing some here
    End Enum
#End Region

#Region "Form Events"
    Public Delegate Sub WinEventDelegate(ByVal hWinEventHook As IntPtr, ByVal eventType As UInteger, ByVal hWnd As IntPtr, ByVal idObject As Integer, ByVal idChild As Integer, ByVal dwEventThread As UInteger, ByVal dwmsEventTime As UInteger)
    Public Delegate Sub ActiveWindowChangedHandler(ByVal sender As Object, ByVal windowTitle As String, ByVal windowClass As String, ByVal hWnd As IntPtr)
    Public Event ActiveWindowChanged As ActiveWindowChangedHandler

    Private __Loaded As Integer = 0
    Private __Hotkeys As New Hotkeys
    Private __SettingsChanged As Boolean = False
    Private __Cursor = New Cursor(Cursor.Current.Handle)
    Private __CurrentGame As String = Nothing
    Private __TaskSchedeuler As New Scheduler("Better Fullscreen", "cmd430", "Starts Better Fullscreen on Logon", Nothing, Nothing, False)
    Private __hWinHook As IntPtr
    Private __winEventProc As WinEventDelegate

    Private Sub BetterFullscreen_Load(sender As Object, e As EventArgs) Handles Me.Load
        __winEventProc = New WinEventDelegate(AddressOf WinEventProc)
        __hWinHook = SetWinEventHook(HOOK_EVENT.EVENT_SYSTEM_FOREGROUND, HOOK_EVENT.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, __winEventProc, 0, 0, WIN_EVENT.WINEVENT_OUTOFCONTEXT)

        AddHandler __Hotkeys.KeyPressed, AddressOf Add_Game
        __Hotkeys.RegisterHotKey(ReadINI(Config, "SETTINGS", "modifier", ModifierKey.Alt), ReadINI(Config, "SETTINGS", "hotkey", Keys.F3))

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
    Private Sub BetterFullscreen_ActiveWindowChanged(ByVal sender As Object, ByVal windowTitle As String, ByVal windowClass As String, ByVal hWnd As IntPtr) Handles Me.ActiveWindowChanged
        Debug.WriteLine("Active Window Title: " & windowTitle & ", Active Window Class: " & windowClass)
        DoWork(windowTitle, windowClass, hWnd)
    End Sub
    Private Sub BetterFullscreen_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        If __Loaded = 0 Then
            Hide()
            __Loaded = 1
        End If
    End Sub
    Private Sub Button_Save_Click(sender As Object, e As EventArgs) Handles Button_Save.Click
        SaveGameSettings()
    End Sub
    Private Sub Button_Remove_Click(sender As Object, e As EventArgs) Handles Button_Remove.Click
        Dim Game As KeyValuePair(Of String, WindowData) = Games.Single(Function(ByVal G) G.Key = ComboBox_Games.SelectedItem)
        Dim confirm As Integer = MessageBox.Show("Remove Profile '" & Game.Key & "'?", "Confirm Profile Removal", MessageBoxButtons.OKCancel)
        If confirm = DialogResult.OK Then
            RemoveINISection(Config, Game.Key)
            LogEvent("Removed '" & Game.Key & "' settings")
            ComboBox_Games.Items.RemoveAt(ComboBox_Games.SelectedIndex)
            Games.Remove(Game.Key)
            LoadGameSettings()
        End If
    End Sub
    Private Sub Button_Reload_Click(sender As Object, e As EventArgs) Handles Button_Reload.Click
        LoadGameSettings(ComboBox_Games.SelectedIndex)
    End Sub
    Private Sub ComboBox_Games_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_Games.SelectedIndexChanged
        LoadGameSettings(ComboBox_Games.SelectedIndex)
    End Sub
    Private Sub LoadGameSettings(Optional ByVal SelectedGame As String = Nothing)
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
        If ComboBox_Games.SelectedIndex > -1 Then
            Dim Game As KeyValuePair(Of String, WindowData) = Games.Single(Function(ByVal G) G.Key = ComboBox_Games.SelectedItem)
            TextBox_Title.Text = Game.Value.Title
            TextBox_Class.Text = Game.Value.Class
            NumericUpDown_Width.Value = Game.Value.Size.Width
            NumericUpDown_Height.Value = Game.Value.Size.Height
            NumericUpDown_Top.Value = Game.Value.Location.Y
            NumericUpDown_Left.Value = Game.Value.Location.X
            NumericUpDown_Delay.Value = Game.Value.Delay
            CheckBox_CaptureMouse.Checked = Game.Value.CaptureMouse
            CheckBox_ForceTopMost.Checked = Game.Value.ForceTopMost
            CheckBox_ProfileEnabled.Checked = Game.Value.ProfileEnabled
            LogEvent("Loaded '" & Game.Key & "' settings")
        End If
    End Sub
    Private Sub SaveGameSettings()
        If ComboBox_Games.SelectedIndex > -1 Then
            Dim Game As KeyValuePair(Of String, WindowData) = Games.Single(Function(ByVal G) G.Key = ComboBox_Games.SelectedItem)
            WriteINI(Config, Game.Key, "title", TextBox_Title.Text)
            WriteINI(Config, Game.Key, "class", TextBox_Class.Text)
            WriteINI(Config, Game.Key, "size", NumericUpDown_Width.Value & "x" & NumericUpDown_Height.Value)
            WriteINI(Config, Game.Key, "location", NumericUpDown_Left.Value & "x" & NumericUpDown_Top.Value)
            WriteINI(Config, Game.Key, "delay", NumericUpDown_Delay.Value)
            WriteINI(Config, Game.Key, "capture-mouse", CheckBox_CaptureMouse.Checked)
            WriteINI(Config, Game.Key, "force-topmost", CheckBox_ForceTopMost.Checked)
            WriteINI(Config, Game.Key, "profile-enabled", CheckBox_ProfileEnabled.Checked)
            Game.Value.Title = TextBox_Title.Text
            Game.Value.Class = TextBox_Class.Text
            Game.Value.Size = New Size(NumericUpDown_Width.Value, NumericUpDown_Height.Value)
            Game.Value.Location = New Point(NumericUpDown_Left.Value, NumericUpDown_Top.Value)
            Game.Value.Delay = NumericUpDown_Delay.Value
            Game.Value.CaptureMouse = CheckBox_CaptureMouse.Checked
            Game.Value.ProfileEnabled = CheckBox_ProfileEnabled.Checked
            Game.Value.ForceTopMost = CheckBox_ForceTopMost.Checked
            LogEvent("Saved '" & Game.Key & "' settings")
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
    Private Sub Button_ReloadApp_Click(sender As Object, e As EventArgs) Handles Button_ReloadApp.Click
        LoadSettings()
    End Sub
    Private Sub Button_SaveApp_Click(sender As Object, e As EventArgs) Handles Button_SaveApp.Click
        SaveSettings()
    End Sub
    Private Sub LoadSettings()
        ComboBox_Key.SelectedIndex = CType(ReadINI(Config, "SETTINGS", "hotkey", 114), Integer) - 112
        ComboBox_Modifier.SelectedIndex = ReadINI(Config, "SETTINGS", "modifier", ModifierKey.Alt)
        Dim [size] As String() = ReadINI(Config, "SETTINGS", "default_size", "800x600").Split("x"c)
        Dim [location] As String() = ReadINI(Config, "SETTINGS", "default_location", "0x0").Split("x"c)
        NumericUpDown_DefaultWidth.Value = [size](0)
        NumericUpDown_DefaultHeight.Value = [size](1)
        NumericUpDown_DefaultTop.Value = [location](1)
        NumericUpDown_DefaultLeft.Value = [location](0)
        LogEvent("Loaded application settings")
    End Sub
    Private Sub SaveSettings()
        WriteINI(Config, "SETTINGS", "hotkey", ComboBox_Key.SelectedIndex + 112)
        WriteINI(Config, "SETTINGS", "modifier", ComboBox_Modifier.SelectedIndex)
        WriteINI(Config, "SETTINGS", "default_size", NumericUpDown_DefaultWidth.Value & "x" & NumericUpDown_DefaultHeight.Value)
        WriteINI(Config, "SETTINGS", "default_location", NumericUpDown_DefaultLeft.Value & "x" & NumericUpDown_DefaultTop.Value)
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
            LoadSettings()
            LoadGameSettings()
            If ComboBox_Games.SelectedIndex > -1 And __CurrentGame IsNot Nothing Then
                ComboBox_Games.SelectedItem = __CurrentGame
            End If
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

    Private ReadOnly Config As String = Application.ExecutablePath.Replace(".exe", ".ini")
    Public Games As New Dictionary(Of String, WindowData)
    Private WindowsScaleFactor As Int32 = 1

    Public Class WindowData
        Public Property [Title] As String
        Public Property [Class] As String
        Public Property [Size] As Size
        Public Property [Location] As Point
        Public Property [Delay] As Integer
        Public Property [CaptureMouse] As Boolean
        Public Property [ForceTopMost] As Boolean
        Public Property [ProfileEnabled] As Boolean
        Public Property [State] As Integer
    End Class

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
        Dim start As DateTime
        Dim [end] As DateTime
        Dim elapsed As TimeSpan
        LogEvent("Loading Games")
        start = Now
        For Each Section As String In ReadINISections(Config)
            If Not Section = "SETTINGS" Then
                LoadGame(Section)
                ComboBox_Games.Items.Add(Section)
            End If
        Next
        [end] = Now
        elapsed = [end].Subtract(start)
        LogEvent("Loaded all games in " & elapsed.TotalSeconds.ToString("0.000") & "ms")
        LogEvent("Ready")
    End Sub

    Private Sub LogEvent(msg)
        RichTextBox_EventLog.AppendText("[" & Now.ToString("HH:mm:ss") & "] " & msg & vbCrLf)
    End Sub

    Private Sub LoadGame(Game)
        Dim _Title As String = ReadINI(Config, Game, "title", "")
        Dim _Class As String = ReadINI(Config, Game, "class", "")
        Dim _Size As String() = ReadINI(Config, Game, "size", "800x600").Split("x"c)
        Dim _Location As String() = ReadINI(Config, Game, "location", "0x0").Split("x"c)

        If Not _Title = "" Or Not _Class = "" Then
            Games.Add(Game, New WindowData With {
              .Title = If(_Title = "", Nothing, _Title),
              .Class = If(_Class = "", Nothing, _Class),
              .Size = New Size(_Size(0), _Size(1)),
              .Location = New Point(_Location(0), _Location(1)),
              .Delay = ReadINI(Config, Game, "delay", 0),
              .CaptureMouse = ReadINI(Config, Game, "capture-mouse", False),
              .ForceTopMost = ReadINI(Config, Game, "force-topmost", False),
              .ProfileEnabled = ReadINI(Config, Game, "profile-enabled", True),
              .State = 0
            })
            LogEvent("Loaded " & Game)
        Else
            LogEvent("Skipped " & Game)
        End If
    End Sub

    Private Sub DoWork(ByVal windowTitle As String, ByVal windowClass As String, ByVal Window_HWND As IntPtr)
        For Each Game As KeyValuePair(Of String, WindowData) In Games
            ' Part1. Remove whitespace from title/class because the windowClass/windowTitle is trimmed to account for the ini not loading the trailing whitespace, this is only
            '        needed because of the part2 hack to allow FindWindowW to work by setting the class/title of the game to the true class/title (with whitespace) in memory so
            '        we can remove the TOPMOST flag from the window, kinds ugly but it seems to do the trick
            Dim GameClass = Game.Value.Class?.Trim()
            Dim GameTitle = Game.Value.Title?.Trim()

            If Game.Value.ProfileEnabled And ((Not GameClass = "" And GameClass = windowClass) Or GameClass = "") And ((Not GameTitle = "" And GameTitle = windowTitle) Or GameTitle = "") Then
                If Game.Value.State = 0 Then
                    LogEvent(Game.Key & " started")
                    If Game.Value.Delay > 0 Then
                        LogEvent("delaying actions for " & Game.Value.Delay & "ms")
                        Thread.Sleep(Game.Value.Delay)
                    End If
                    LogEvent("setting WS_VISIBLE")
                    SetWindowLong(Window_HWND, GWL.STYLE, WS.VISIBLE)
                    LogEvent("resizing window " & Game.Value.Size.ToString())
                    LogEvent("repositioning window " & Game.Value.Location.ToString())
                    SetWindowPos(Window_HWND, HWND.TOP, Game.Value.Location.X, Game.Value.Location.Y, Game.Value.Size.Width / WindowsScaleFactor, Game.Value.Size.Height / WindowsScaleFactor, SWP.FRAMECHANGED)
                    If Game.Value.CaptureMouse Then
                        __Cursor.Clip = New Rectangle(Game.Value.Location, Game.Value.Size)
                    End If
                    Game.Value.State = 1
                    __CurrentGame = Game.Key

                    ' Part2. Hack to fix for windows ending with whitespace by saving the class/title with any leading/trailing whitespace 
                    '        back into the game object, fixes FindWindowW not working with the truncated class/titles of some games (example: Back 4 Blood)
                    Dim hWndTitle As New StringBuilder("", 256)
                    GetWindowText(Window_HWND, hWndTitle, 256)
                    Game.Value.Title = hWndTitle.ToString()
                End If
                If Game.Value.State > 0 Then
                    Dim rect As New RECT
                    Dim Window_Rect = New Rectangle()

                    GetWindowRect(Window_HWND, rect)

                    Window_Rect.X = rect.left
                    Window_Rect.Y = rect.top
                    Window_Rect.Width = rect.right - rect.left
                    Window_Rect.Height = rect.bottom - rect.top

                    Dim correctPos = New Point(Window_Rect.X, Window_Rect.Y) = Game.Value.Location
                    Dim correctSize = New Size(Window_Rect.Width, Window_Rect.Height) = New Size(Game.Value.Size.Width / WindowsScaleFactor, Game.Value.Size.Height / WindowsScaleFactor)

                    If Not correctPos Or Not correctSize Then
                        LogEvent("resizing window " & Game.Value.Size.ToString())
                        LogEvent("repositioning window " & Game.Value.Location.ToString())
                        SetWindowPos(Window_HWND, HWND.TOP, Game.Value.Location.X, Game.Value.Location.Y, Game.Value.Size.Width / WindowsScaleFactor, Game.Value.Size.Height / WindowsScaleFactor, SWP.FRAMECHANGED)
                    End If

                    If Not GetWindowLong(Window_HWND, GWL.STYLE) = 335544320 Then
                        LogEvent("setting WS_VISIBLE")
                        SetWindowLong(Window_HWND, GWL.STYLE, WS.VISIBLE)
                    End If
                End If
                If Game.Value.State = 1 Then
                    LogEvent(Game.Key & " has focus")
                    If Game.Value.ForceTopMost And Not GetWindowLong(Window_HWND, GWL.EXSTYLE) = 262152 Then
                        LogEvent("setting HWND_TOPMOST")
                        SetWindowPos(Window_HWND, HWND.TOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE)
                    End If
                    If Game.Value.CaptureMouse Then
                        LogEvent("locking mouse to game window location")
                        __Cursor.Clip = New Rectangle(Game.Value.Location, Game.Value.Size)
                    End If
                    Game.Value.State = 2
                End If
            Else
                If __CurrentGame IsNot Nothing Then
                    Dim Game_HWND = FindWindowW(Games.Item(__CurrentGame).Class, Games.Item(__CurrentGame).Title)
                    If Game_HWND <> IntPtr.Zero Then
                        If Game.Value.State = 2 Then
                            LogEvent(Game.Key & " lost focus")
                            If Game.Value.ForceTopMost Then
                                LogEvent("setting HWND_NOTOPMOST")
                                SetWindowPos(Game_HWND, HWND.NOTOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE)
                            End If
                            If Game.Value.CaptureMouse Then
                                LogEvent("releasing mouse lock from game window location")
                                __Cursor.Clip = New Rectangle(New Point(0, 0), SystemInformation.VirtualScreen.Size)
                            End If
                            Game.Value.State = 1
                        End If
                    Else
                        If Game.Value.State > 0 Then
                            LogEvent(Game.Key & " exited")
                            If Game.Value.CaptureMouse And Game.Value.State = 2 Then
                                LogEvent("releasing mouse lock from game window location")
                                __Cursor.Clip = New Rectangle(New Point(0, 0), SystemInformation.VirtualScreen.Size)
                            End If
                            Game.Value.State = 0
                            __CurrentGame = Nothing
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub Add_Game()
        Dim hWnd = GetForegroundWindow()
        Dim hWndTitle As New StringBuilder("", 256)
        GetWindowText(hWnd, hWndTitle, 256)
        Dim hWndClass As New StringBuilder("", 256)
        GetClassName(hWnd, hWndClass, 256)
        Dim _size As String() = ReadINI(Config, "SETTINGS", "default_size", "800x600").Split("x"c)
        Dim _location As String() = ReadINI(Config, "SETTINGS", "default_location", "0x0").Split("x"c)
        Dim [size] As New Size(_size(0), _size(1))
        Dim [location] As New Point(_location(0), _location(1))
        Dim [title] = hWndTitle.ToString().Trim()
        Dim [class] = hWndClass.ToString().Trim()
        Dim [section] = [title].Replace("[", "(").Replace("]", ")").Trim()
        If Not [title] = "" Then
            If Not Games.ContainsKey([section]) Then
                WriteINI(Config, [section], "title", [title])
                WriteINI(Config, [section], "class", [class])
                WriteINI(Config, [section], "size", [size].Width & "x" & [size].Height)
                WriteINI(Config, [section], "location", [location].X & "x" & [location].Y)
                WriteINI(Config, [section], "delay", 0)
                WriteINI(Config, [section], "capture-mouse", False)
                WriteINI(Config, [section], "force-topmost", False)
                WriteINI(Config, [section], "profile-enabled", True)
                LogEvent([section] & " added")
                LogEvent("size " & [size].ToString)
                LogEvent("location " & [location].ToString())
                LoadGame([section])
                ComboBox_Games.Items.Add([section])
            End If
        End If
    End Sub

    Private Sub WinEventProc(ByVal hWinEventHook As IntPtr, ByVal eventType As UInteger, ByVal hWnd As IntPtr, ByVal idObject As Integer, ByVal idChild As Integer, ByVal dwEventThread As UInteger, ByVal dwmsEventTime As UInteger)
        If eventType = HOOK_EVENT.EVENT_SYSTEM_FOREGROUND Then
            Dim hWndTitleBuilder As New StringBuilder("", 256)
            Dim hWndClassBuilder As New StringBuilder("", 256)
            Dim hWndTitle As String = ""
            Dim hWndClass As String = ""

            GetWindowText(hWnd, hWndTitleBuilder, 256)
            GetClassName(hWnd, hWndClassBuilder, 256)

            hWndTitle = hWndTitleBuilder.ToString().Trim()
            hWndClass = hWndClassBuilder.ToString().Trim()

            RaiseEvent ActiveWindowChanged(Me, hWndTitle, hWndClass, hWnd)
        End If
    End Sub

End Class