Imports System.Runtime.InteropServices
Imports System.Text

Module Win32

#Region "Native Methods"

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="FindWindowW", CallingConvention:=CallingConvention.StdCall)>
    Public Function FindWindowW(<MarshalAs(UnmanagedType.LPTStr)> lpClassName As String, <MarshalAs(UnmanagedType.LPTStr)> lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetForegroundWindow", CallingConvention:=CallingConvention.StdCall)>
    Public Function GetForegroundWindow() As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="SetForegroundWindow", CallingConvention:=CallingConvention.StdCall)>
    Public Function SetForegroundWindow(hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="SetWindowLong", CallingConvention:=CallingConvention.StdCall)>
    Public Function SetWindowLong(hWnd As IntPtr, nIndex As Integer, dwNewLong As IntPtr) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetWindowLong", CallingConvention:=CallingConvention.StdCall)>
    Public Function GetWindowLong(hWnd As IntPtr, nIndex As Integer) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="SetWindowPos", CallingConvention:=CallingConvention.StdCall)>
    Public Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr, X As Integer, Y As Integer, cx As Integer, cy As Integer, uFlags As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetWindowText", CallingConvention:=CallingConvention.StdCall)>
    Public Function GetWindowText(hWnd As Integer, lpString As StringBuilder, count As Integer) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetClassName", CallingConvention:=CallingConvention.StdCall)>
    Public Function GetClassName(hWnd As IntPtr, lpClassName As StringBuilder, nMaxCount As Integer) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetWindowRect", CallingConvention:=CallingConvention.StdCall)>
    Public Function GetWindowRect(hWnd As IntPtr, ByRef lpRect As RECT) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="SetWinEventHook", CallingConvention:=CallingConvention.StdCall)>
    Public Function SetWinEventHook(eventMin As UInteger, eventMax As UInteger, hmodWinEventProc As IntPtr, lpfnWinEventProc As WinEventDelegate, idProcess As UInteger, idThread As UInteger, dwFlags As UInteger) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="UnhookWinEvent", CallingConvention:=CallingConvention.StdCall)>
    Public Function UnhookWinEvent(hWinEventHook As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="SendMessage", CallingConvention:=CallingConvention.StdCall)>
    Public Function SendMessage(hWnd As IntPtr, Msg As UInteger, wParam As Integer, lParam As IntPtr) As IntPtr
    End Function

#End Region

    Public Delegate Sub WinEventDelegate(hWinEventHook As IntPtr, eventType As UInteger, hWnd As IntPtr, idObject As Integer, idChild As Integer, dwEventThread As UInteger, dwmsEventTime As UInteger)

#Region "Win32 Structs, Enums & Varibles"

    <StructLayout(LayoutKind.Sequential)>
    Public Structure RECT
        Public left, top, right, bottom As Integer
    End Structure

    <Flags>
    Public Enum ModifierKey As Long
        None = 0
        Alt = 1
        Control = 2
        Shift = 4
        Win = 8
    End Enum

    <Flags>
    Public Enum HWND As Long
        TOP = 0
        BOTTOM = 1
        TOPMOST = -1
        NOTOPMOST = -2
    End Enum

    <Flags>
    Public Enum GWL As Long
        WNDPROC = -4
        HINSTANCE = -6
        HWNDPARENT = -8
        STYLE = -16
        EXSTYLE = -20
        USERDATA = -21
        ID = -12
    End Enum

    <Flags>
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

    <Flags>
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
    <Flags>
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
    <Flags>
    Public Enum WIN_EVENT_FLAGS As UInteger
        WINEVENT_OUTOFCONTEXT = 0 'Events are ASYNC
        WINEVENT_SKIPOWNTHREAD = 1 'Dont call back for events on installer's thread
        WINEVENT_SKIPOWNPROCESS = 2 'Dont call back for events on installer's process
        WINEVENT_INCONTEXT = 4 'Events are SYNC, this causes your dll To be injected into every process
    End Enum
    <Flags>
    Public Enum WIN_EVENT As UInteger
        EVENT_SYSTEM_FOREGROUND = &H3 ' The foreground window has changed.
        EVENT_SYSTEM_CAPTUREEND = &H9 ' A window has lost mouse capture.
        EVENT_SYSTEM_CAPTURESTART = &H8 ' A window has received mouse capture.
        EVENT_SYSTEM_SWITCHSTART = &H14 ' The user has pressed ALT+TAB.
        EVENT_SYSTEM_SWITCHEND = &H15 ' The user has released ALT+TAB.
        EVENT_SYSTEM_MINIMIZESTART = &H16 ' A window object is about to be minimized.
        EVENT_SYSTEM_MINIMIZEEND = &H17 ' A window object is about to be restored.
        EVENT_SYSTEM_DESKTOPSWITCH = &H20 ' The active desktop has been switched.
        ' Im missing a bunch that i probably dont care about
    End Enum

    Public Enum WIN_MESSAGE As UInteger
        WM_SETICON = &H80UI
    End Enum
    Public Enum ICON_SIZE As Integer
        ICON_SMALL = 0
        ICON_BIG = 1
    End Enum

#End Region

End Module