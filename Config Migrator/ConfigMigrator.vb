Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Runtime.InteropServices

Public Module ConfigMigrator

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="WritePrivateProfileString")>
    Public Function WritePrivateProfileString(lpApplicationName As String, lpKeyName As String, lpString As String, lpFileName As String) As Integer
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="WritePrivateProfileSection")>
    Public Function WritePrivateProfileSection(lpKeyName As String, lpString As String, lpFileName As String) As Integer
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True, EntryPoint:="GetPrivateProfileString")>
    Public Function GetPrivateProfileString(lpApplicationName As String, lpKeyName As String, lpDefault As String, lpReturnedString As String, nSize As Integer, lpFileName As String) As Integer
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Ansi, SetLastError:=True, EntryPoint:="GetPrivateProfileSectionNames")>
    Public Function GetPrivateProfileSectionNames(lpszReturnBuffer As IntPtr, nSize As Integer, lpFileName As String) As Integer
    End Function

    Sub Main()

        Dim INIPath = "./Better Fullscreen.ini"
        Dim CONFPath = "./Better Fullscreen.conf"

        Dim Conf = New BetterFullscreenConfig With {
            .Settings = New Settings,
            .Profile = New List(Of Profile)
        }

        For Each Section As String In ReadINISections(INIPath)

            Console.WriteLine(Section)

            If Section = "SETTINGS" Then
                Dim size As String() = ReadINI(INIPath, Section, "default_size", "800x600").Split("x"c)
                Dim location As String() = ReadINI(INIPath, Section, "default_location", "0x0").Split("x"c)

                Conf.Settings.DefaultSize = New Size(size(0), size(1))
                Conf.Settings.DefaultLocation = New Point(location(0), location(1))
                Conf.Settings.Hotkey = CType(ReadINI(INIPath, Section, "hotkey", Keys.F3), Keys)
                Conf.Settings.Modifier = CType(ReadINI(INIPath, Section, "modifier", ModifierKey.Alt), ModifierKey)
            Else
                Dim size As String() = ReadINI(INIPath, Section, "size", "800x600").Split("x"c)
                Dim location As String() = ReadINI(INIPath, Section, "location", "0x0").Split("x"c)

                Conf.Profile.Add(New Profile With {
                    .Size = New Size(size(0), size(1)),
                    .Location = New Point(location(0), location(1)),
                    .Title = ReadINI(INIPath, Section, "title", ""),
                    .Class = ReadINI(INIPath, Section, "class", ""),
                    .Delay = ReadINI(INIPath, Section, "delay", 0),
                    .CaptureMouse = ReadINI(INIPath, Section, "capture-mouse", False),
                    .ForceTopMost = ReadINI(INIPath, Section, "force-topmost", True),
                    .Enabled = ReadINI(INIPath, Section, "profile-enabled", True),
                    .Name = Section
                })
            End If
        Next

        SaveConfig(CONFPath, Conf)
    End Sub

    Public Function ReadINISections(inipath As String) As Specialized.StringCollection
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

    Public Function ReadINI(IniFileName As String, Section As String, ParamName As String, ParamDefault As String) As String
        Dim ParamVal As String = Space$(1024)
        Dim LenParamVal As Long = GetPrivateProfileString(Section, ParamName, ParamDefault, ParamVal, Len(ParamVal), IniFileName)
        ReadINI = Left$(ParamVal, LenParamVal)
    End Function

    Public Sub WriteINI(iniFileName As String, Section As String, ParamName As String, ParamVal As String)
        Dim Result As Integer = WritePrivateProfileString(Section, ParamName, ParamVal, iniFileName)
    End Sub

    Public Sub RemoveINISection(IniFileName As String, Section As String)
        Dim Result As Integer = WritePrivateProfileSection(Section, Nothing, IniFileName)
    End Sub

    <XmlRoot(ElementName:="Better-Fullscreen")>
    Public Class BetterFullscreenConfig
        <XmlElement(ElementName:="Settings")>
        Public Property Settings As Settings
        <XmlElement(ElementName:="Profile")>
        Public Property Profile As List(Of Profile)
    End Class

    <XmlRoot(ElementName:="Settings")>
    Public Class Settings
        <XmlElement(ElementName:="Hotkey")>
        Public Property Hotkey As Keys = Keys.F3
        <XmlElement(ElementName:="Modifier")>
        Public Property Modifier As ModifierKey = ModifierKey.Alt
        <XmlElement(ElementName:="Default-Size")>
        Public Property DefaultSize As Size = New Size(800, 600)
        <XmlElement(ElementName:="Default-Location")>
        Public Property DefaultLocation As Point = New Point(0, 0)
    End Class

    <XmlRoot(ElementName:="Profile")>
    Public Class Profile
        <XmlAttribute(AttributeName:="name")>
        Public Property Name As String = ""
        <XmlAttribute(AttributeName:="enabled")>
        Public Property Enabled As Boolean = True
        <XmlElement(ElementName:="Title")>
        Public Property Title As String = ""
        <XmlElement(ElementName:="Class")>
        Public Property [Class] As String = ""
        <XmlElement(ElementName:="Size")>
        Public Property Size As Size = New Size(800, 600)
        <XmlElement(ElementName:="Location")>
        Public Property Location As Point = New Point(0, 0)
        <XmlElement(ElementName:="Delay")>
        Public Property Delay As Integer = 0
        <XmlElement(ElementName:="Capture-Mouse")>
        Public Property CaptureMouse As Boolean = False
        <XmlElement(ElementName:="Force-Topmost")>
        Public Property ForceTopMost As Boolean = True
    End Class

    Public Enum ModifierKey As UInteger
        Alt = 1
        Control = 2
        Shift = 4
        Win = 8
    End Enum

    Public Sub SaveConfig(configPath As String, ByRef config As BetterFullscreenConfig)
        Using stream As New StreamWriter(File.Open(configPath, FileMode.Create), Encoding.UTF8)
            Dim serializer As New XmlSerializer(GetType(BetterFullscreenConfig))

            serializer.Serialize(stream, config, New XmlSerializerNamespaces({XmlQualifiedName.Empty}))
        End Using
    End Sub

End Module
