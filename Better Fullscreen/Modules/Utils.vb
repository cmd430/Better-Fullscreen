Imports System.Text
Imports Microsoft.Win32
Imports Microsoft.Win32.Registry

Module Utils

    Public Sub ToggleWindowState(window As Form, Optional state As FormWindowState = -1)
        If state <> -1 Then
            window.WindowState = state
            window.Show()
            Exit Sub
        End If
        If window.WindowState = FormWindowState.Normal Then
            window.WindowState = FormWindowState.Minimized
        Else
            window.WindowState = FormWindowState.Normal
        End If
    End Sub

    Public Function GetWindowTitle(HWND As IntPtr, Optional clean As Boolean = True) As String
        Dim title As New StringBuilder("", 256)

        GetWindowText(HWND, title, 256)

        Return If(clean, title.ToString().Trim(), title.ToString())
    End Function

    Public Function GetWindowClass(HWND As IntPtr, Optional clean As Boolean = True) As String
        Dim [class] As New StringBuilder("", 256)

        GetClassName(HWND, [class], 256)

        Return If(clean, [class].ToString().Trim(), [class].ToString())
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
