Imports System.Runtime.InteropServices
Imports System.Text

Module INI

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="WritePrivateProfileString")>
    Private Function WritePrivateProfileString(ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="WritePrivateProfileSection")>
    Private Function WritePrivateProfileSection(ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetPrivateProfileString")>
    Private Function GetPrivateProfileString(ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Ansi, SetLastError:=True, EntryPoint:="GetPrivateProfileSectionNames")>
    Private Function GetPrivateProfileSectionNames(ByVal lpszReturnBuffer As IntPtr, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    End Function
    Public Function ReadINISections(ByVal inipath As String) As Specialized.StringCollection
        Dim profiles As New Specialized.StringCollection
        Dim ptr As IntPtr = Marshal.StringToHGlobalAnsi(New String(vbNullChar, 32767))
        Dim len As Integer = GetPrivateProfileSectionNames(ptr, 32767, inipath)
        Dim buff As String = Marshal.PtrToStringAnsi(ptr, len)
        Marshal.FreeHGlobal(ptr)
        Dim sb As New StringBuilder
        For ii As Integer = 0 To len - 1
            Dim c As Char = buff.Chars(ii)
            If c = vbNullChar Then
                profiles.Add(sb.ToString)
                sb.Length = 0
            Else
                sb.Append(c)
            End If
        Next
        Return profiles
    End Function
    Public Function ReadINI(ByVal IniFileName As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamDefault As String) As String
        Dim ParamVal As String = Space$(1024)
        Dim LenParamVal As Long = GetPrivateProfileString(Section, ParamName, ParamDefault, ParamVal, Len(ParamVal), IniFileName)
        ReadINI = Left$(ParamVal, LenParamVal)
    End Function
    Public Sub WriteINI(ByVal iniFileName As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamVal As String)
        Dim Result As Integer = WritePrivateProfileString(Section, ParamName, ParamVal, iniFileName)
    End Sub
    Public Sub RemoveINISection(ByVal IniFileName As String, ByVal Section As String)
        Dim Result As Integer = WritePrivateProfileSection(Section, Nothing, IniFileName)
    End Sub

End Module