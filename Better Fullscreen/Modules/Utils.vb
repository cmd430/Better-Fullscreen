Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32
Imports Microsoft.Win32.Registry

Module Utils

    Public Sub ToggleWindowState(window As Form, Optional state As FormWindowState = -1)
        If state = -1 Then
            If window.WindowState = FormWindowState.Normal Then
                state = FormWindowState.Minimized
            Else
                state = FormWindowState.Normal
            End If
        End If

        Dim IsVisible = (state = FormWindowState.Normal)

        window.WindowState = state
        window.ShowInTaskbar = IsVisible
        window.Visible = IsVisible

        SetTitleBarTheme(window.Handle)
    End Sub

    Public Function GetWindowTitle(HWND As IntPtr, Optional Unsafe As Boolean = False) As String
        Dim title As New StringBuilder("", 256)

        GetWindowText(HWND, title, 256)

        Return If(Unsafe, title.ToString(), title.ToString().Trim())
    End Function

    Public Function GetWindowClass(HWND As IntPtr, Optional Unsafe As Boolean = False) As String
        Dim [class] As New StringBuilder("", 256)

        GetClassName(HWND, [class], 256)

        Return If(Unsafe, [class].ToString(), [class].ToString().Trim())
    End Function

    Public Function GetWindowRectangle(HWND As IntPtr) As Rectangle
        Dim rect As New RECT
        If GetWindowRect(HWND, rect) Then
            Return New Rectangle() With {
            .X = rect.left,
            .Y = rect.top,
            .Width = rect.right - rect.left,
            .Height = rect.bottom - rect.top
        }
        End If
        Return New Rectangle()
    End Function

    Public Function GetRadioButton(panel As Panel) As RadioButton
        Return panel.Controls.OfType(Of RadioButton).Where(Function(r) r.Checked = True).FirstOrDefault()
    End Function

    Public Sub SetRadioButton(panel As Panel, value As Integer)
        panel.Controls.OfType(Of RadioButton).Where(Function(r) CType(r.Tag, Integer) = value).FirstOrDefault().Checked = True
        panel.Controls.OfType(Of RadioButton).Where(Function(r) CType(r.Tag, Integer) <> value).FirstOrDefault().Checked = False
    End Sub

    Public Function GetWindowsTheme() As WindowsTheme
        Using Key As RegistryKey = CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            If Key IsNot Nothing Then
                Dim Value As Object = Key.GetValue("AppsUseLightTheme")
                If Value IsNot Nothing Then
                    Return If(CInt(Value) > 0, WindowsTheme.Light, WindowsTheme.Dark)
                End If
            End If

            Return WindowsTheme.Light
        End Using
    End Function

    Public Sub SetTitleBarTheme(HWND As IntPtr)
        If GetWindowsTheme() = WindowsTheme.Light Then
            DwmSetWindowAttribute(HWND, DwmWindowAttribute.UseImmersiveDarkMode, &H0, Marshal.SizeOf(GetType(Boolean)))
        Else
            DwmSetWindowAttribute(HWND, DwmWindowAttribute.UseImmersiveDarkMode, &H1, Marshal.SizeOf(GetType(Boolean)))
        End If
        DwmSetWindowAttribute(HWND, DwmWindowAttribute.SystemBackdropType, DwmWASystemBackdropType.Mica, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(HWND, DwmWindowAttribute.WindowCornerPreference, DwmWindowCornerPreference.SemiRound, Marshal.SizeOf(GetType(Integer)))
        DwmExtendFrameIntoClientArea(HWND, New MARGINS With {
            .topHeight = 23,
            .leftWidth = 3,
            .bottomHeight = 3,
            .rightWidth = 3
        })
    End Sub

    Public Function GetWindowsScaleFactor() As Int32
        Using Key As RegistryKey = CurrentUser.OpenSubKey("Control Panel\Desktop\WindowMetrics")
            If Key IsNot Nothing Then
                Dim ADPI As Object = Key.GetValue("AppliedDPI")
                If ADPI IsNot Nothing Then
                    Return Convert.ToInt32(ADPI) / 96
                End If
            End If
            Return 1
        End Using
    End Function

    Public Function IsWindowTopMost(HWND As IntPtr) As Boolean
        Return (GetWindowLong(HWND, GWL.EXSTYLE) And WS_EX.TOPMOST) = WS_EX.TOPMOST
    End Function

    Public Function IsCursorClipped() As Boolean
        Return Cursor.Clip = New Rectangle(New Point(0, 0), SystemInformation.VirtualScreen.Size)
    End Function

End Module
