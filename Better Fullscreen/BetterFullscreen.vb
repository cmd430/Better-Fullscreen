Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32
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
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="SetWindowPos")>
    Private Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As Integer) As Boolean
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetWindowText")>
    Private Shared Function GetWindowText(hWnd As Integer, text As StringBuilder, count As Long) As Long
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetClassName")>
    Public Shared Function GetClassName(ByVal hWnd As IntPtr, ByVal lpClassName As String, ByVal nMaxCount As Long) As Long
    End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetWindowRect")>
    Private Shared Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As <MarshalAs(UnmanagedType.Bool)> Boolean
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
#End Region

#Region "Form Events"
    Private __Loaded As Integer = 0
    Private __Hotkeys As New Hotkeys
    Private __SettingsChanged As Boolean = False
    Private __Cursor = New Cursor(Cursor.Current.Handle)

    Private Sub WarframeBetterFullscreen_Load(sender As Object, e As EventArgs) Handles Me.Load
        AddHandler __Hotkeys.KeyPressed, AddressOf Add_Game
        __Hotkeys.RegisterHotKey(ReadINI(Config, "SETTINGS", "modifier", ModifierKey.Alt), ReadINI(Config, "SETTINGS", "hotkey", Keys.F3))
        If LoadWithWindows() Then
            CheckBox_startWithWindows.Checked = True
        End If
        AddHandler CheckBox_startWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged
        Init()
    End Sub
    Private Sub Timer_Scanner_Tick(sender As Object, e As EventArgs) Handles Timer_Scanner.Tick
        DoWork()
    End Sub
    Private Sub WarframeBetterFullscreen_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
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
        RemoveINISection(Config, Game.Key)
        RichTextBox_EventLog.AppendText("Removed '" & Game.Key & "' settings" & vbCrLf)
        ComboBox_Games.Items.RemoveAt(ComboBox_Games.SelectedIndex)
        Games.Remove(Game.Key)
        LoadGameSettings()
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
            CheckBox_ProfileEnabled.Checked = Game.Value.Enabled
            RichTextBox_EventLog.AppendText("Loaded '" & Game.Key & "' settings" & vbCrLf)
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
            WriteINI(Config, Game.Key, "profile-enabled", CheckBox_ProfileEnabled.Checked)
            Game.Value.Title = TextBox_Title.Text
            Game.Value.Class = TextBox_Class.Text
            Game.Value.Size = New Size(NumericUpDown_Width.Value, NumericUpDown_Height.Value)
            Game.Value.Location = New Point(NumericUpDown_Left.Value, NumericUpDown_Top.Value)
            Game.Value.Delay = NumericUpDown_Delay.Value
            Game.Value.CaptureMouse = CheckBox_CaptureMouse.Checked
            Game.Value.Enabled = CheckBox_CaptureMouse.Checked
            RichTextBox_EventLog.AppendText("Saved '" & Game.Key & "' settings" & vbCrLf)
            __SettingsChanged = True
        End If
    End Sub
    Public Function LoadWithWindows() As Boolean
        Dim key As RegistryKey = CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
        If key Is Nothing Then
            Return False
        End If

        Dim value As Object = key.GetValue(Process.GetCurrentProcess().MainModule.FileName)
        If TypeOf value Is String Then
            Return value.StartsWith("""" & Application.ExecutablePath & """")
        End If

        Return False
    End Function
    Private Sub CheckBox_startWithWindows_CheckedChanged(sender As Object, e As EventArgs)
        RemoveHandler CheckBox_startWithWindows.CheckedChanged, AddressOf CheckBox_startWithWindows_CheckedChanged
        If LoadWithWindows() Then
            Dim key As RegistryKey = CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
            key.DeleteValue(Process.GetCurrentProcess().MainModule.FileName, False)
            CheckBox_startWithWindows.Checked = False
        Else
            Dim key As RegistryKey = CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
            key.SetValue(Process.GetCurrentProcess().MainModule.FileName, """" & Application.ExecutablePath & """")
            CheckBox_startWithWindows.Checked = True
        End If
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
        RichTextBox_EventLog.AppendText("Loaded application settings" & vbCrLf)
    End Sub
    Private Sub SaveSettings()
        WriteINI(Config, "SETTINGS", "hotkey", ComboBox_Key.SelectedIndex + 112)
        WriteINI(Config, "SETTINGS", "modifier", ComboBox_Modifier.SelectedIndex)
        WriteINI(Config, "SETTINGS", "default_size", NumericUpDown_DefaultWidth.Value & "x" & NumericUpDown_DefaultHeight.Value)
        WriteINI(Config, "SETTINGS", "default_location", NumericUpDown_DefaultLeft.Value & "x" & NumericUpDown_DefaultTop.Value)
        RichTextBox_EventLog.AppendText("Saved application settings" & vbCrLf)
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
            Show()
            Focus()
        End If
    End Sub
    Private Sub ReloadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReloadToolStripMenuItem.Click
        Application.Restart()
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
        End If
    End Sub

#End Region

    Private ReadOnly Config As String = Application.ExecutablePath.Replace(".exe", ".ini")
    Public Games As New Dictionary(Of String, WindowData)

    Public Class WindowData
        Public Property [Title] As String
        Public Property [Class] As String
        Public Property [Size] As Size
        Public Property [Location] As Point
        Public Property [Delay] As Integer
        Public Property [CaptureMouse] As Boolean
        Public Property [State] As Integer
        Public Property [Enabled] As Boolean
    End Class

    Private Sub Init()
        RichTextBox_EventLog.AppendText("Loading Games" & vbCrLf)
        For Each Section As String In ReadINISections(Config)
            If Not Section = "SETTINGS" Then
                LoadGame(Section)
                ComboBox_Games.Items.Add(Section)
            End If
        Next
        RichTextBox_EventLog.AppendText("Ready" & vbCrLf)
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
              .Enabled = ReadINI(Config, Game, "profile-enabled", True),
              .State = 0
            })
            RichTextBox_EventLog.AppendText("Loaded " & Game & vbCrLf)
        Else
            RichTextBox_EventLog.AppendText("Skipped " & Game & vbCrLf)
        End If
    End Sub

    Private Sub DoWork()
        For Each Game As KeyValuePair(Of String, WindowData) In Games
            Dim Window_HWND As IntPtr = FindWindowW(Game.Value.Class, Game.Value.Title)
            If Window_HWND <> IntPtr.Zero And Game.Value.Enabled Then
                If Game.Value.State = 0 Then
                    RichTextBox_EventLog.AppendText(Game.Key & " started" & vbCrLf)
                    RichTextBox_EventLog.AppendText("setting WS_VISIBLE" & vbCrLf)
                    RichTextBox_EventLog.AppendText("resizing window " & Game.Value.Size.ToString() & vbCrLf)
                    RichTextBox_EventLog.AppendText("repositioning window " & Game.Value.Location.ToString() & vbCrLf)
                    If Game.Value.Delay > 0 Then
                        Thread.Sleep(Game.Value.Delay)
                    End If
                    SetWindowLong(Window_HWND, GWL.STYLE, WS.VISIBLE)
                    SetWindowPos(Window_HWND, HWND.TOP, Game.Value.Location.X, Game.Value.Location.Y, Game.Value.Size.Width, Game.Value.Size.Height, SWP.FRAMECHANGED)
                    If Game.Value.CaptureMouse Then
                        __Cursor.Clip = New Rectangle(Game.Value.Location, Game.Value.Size)
                    End If
                    Game.Value.State = 1
                End If
                If Window_HWND = GetForegroundWindow() Then
                    If Game.Value.State = 1 Then
                        RichTextBox_EventLog.AppendText(Game.Key & " has focus" & vbCrLf)
                        RichTextBox_EventLog.AppendText("setting HWND_TOPMOST" & vbCrLf)
                        SetWindowPos(Window_HWND, HWND.TOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE)
                        If Game.Value.CaptureMouse Then
                            __Cursor.Clip = New Rectangle(Game.Value.Location, Game.Value.Size)
                        End If
                        Game.Value.State = 2
                    End If
                Else
                    If Game.Value.State = 2 Then
                        RichTextBox_EventLog.AppendText(Game.Key & " lost focus" & vbCrLf)
                        RichTextBox_EventLog.AppendText("setting HWND_NOTOPMOST" & vbCrLf)
                        SetWindowPos(Window_HWND, HWND.NOTOPMOST, 0, 0, 0, 0, SWP.NOMOVE Or SWP.NOSIZE)
                        If Game.Value.CaptureMouse Then
                            __Cursor.Clip = New Rectangle(New Point(0, 0), SystemInformation.VirtualScreen.Size)
                        End If
                        Game.Value.State = 1
                    End If
                End If
            Else
                If Game.Value.State > 0 Then
                    RichTextBox_EventLog.AppendText(Game.Key & " exited" & vbCrLf)
                    If Game.Value.CaptureMouse Then
                        __Cursor.Clip = New Rectangle(New Point(0, 0), SystemInformation.VirtualScreen.Size)
                    End If
                    Game.Value.State = 0
                End If
            End If
        Next
    End Sub

    Private Sub Add_Game()
        Dim hWnd = GetForegroundWindow()
        Dim hWndTitle As New StringBuilder
        Dim [class] As String = ""
        Dim _size As String() = ReadINI(Config, "SETTINGS", "default_size", "800x600").Split("x"c)
        Dim _location As String() = ReadINI(Config, "SETTINGS", "default_location", "0x0").Split("x"c)
        Dim [size] As New Size(_size(0), _size(1))
        Dim [location] As New Point(_location(0), _location(1))
        GetWindowText(hWnd, hWndTitle, 128)
        GetClassName(hWnd, [class], 128)
        Dim [title] = hWndTitle.ToString()
        If Not [title] = "" Then
            If Not Games.ContainsKey([title]) Then
                WriteINI(Config, [title], "title", [title])
                WriteINI(Config, [title], "class", [class])
                WriteINI(Config, [title], "size", [size].Width & "x" & [size].Height)
                WriteINI(Config, [title], "location", [location].X & "x" & [location].Y)
                WriteINI(Config, [title], "delay", 0)
                WriteINI(Config, [title], "capture-mouse", False)
                WriteINI(Config, [title], "profile-enabled", True)
                RichTextBox_EventLog.AppendText([title] & " added" & vbCrLf)
                RichTextBox_EventLog.AppendText("size " & [size].ToString & vbCrLf)
                RichTextBox_EventLog.AppendText("location " & [location].ToString() & vbCrLf)
                LoadGame([title])
                ComboBox_Games.Items.Add([title])
            End If
        End If
    End Sub

End Class