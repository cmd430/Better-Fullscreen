Imports System.Text

Module Utils

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

    Public Function GetRadioButton(panel As Panel) As RadioButton
        Return panel.Controls.OfType(Of RadioButton).Where(Function(r) r.Checked = True).FirstOrDefault()
    End Function

    Public Sub SetRadioButton(panel As Panel, value As Integer)
        panel.Controls.OfType(Of RadioButton).Where(Function(r) CType(r.Tag, Integer) = value).FirstOrDefault().Checked = True
        panel.Controls.OfType(Of RadioButton).Where(Function(r) CType(r.Tag, Integer) <> value).FirstOrDefault().Checked = False
    End Sub


End Module
