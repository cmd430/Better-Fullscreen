Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization

Public Module Config

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
        <XmlIgnore>
        Public Property ConfigPath As String = ""
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
        <XmlIgnore>
        Public Property UnsafeTitle As String = ""
        <XmlIgnore>
        Public Property UnsafeClass As String = ""
        <XmlIgnore>
        Public Property State As GameState = GameState.None
        <XmlIgnore>
        Public Property IsCurrentProfile As Boolean = False
    End Class

    Public Enum GameState As Integer
        None = 0
        Started = 1
        Focused = 2
        Unfocused = 3
    End Enum

    Public Function LoadConfig(configPath As String) As BetterFullscreenConfig
        If Not File.Exists(configPath) Then
            Return New BetterFullscreenConfig With {
                .Settings = New Settings,
                .Profile = New List(Of Profile)
            }
        End If

        Using stream As New StreamReader(configPath)
            Dim serializer As New XmlSerializer(GetType(BetterFullscreenConfig), New XmlRootAttribute("Better-Fullscreen"))
            Dim config = CType(serializer.Deserialize(stream), BetterFullscreenConfig)

            config.Settings.ConfigPath = configPath

            Return config
        End Using
    End Function

    Public Sub SaveConfig(configPath As String, ByRef config As BetterFullscreenConfig)
        Using stream As New StreamWriter(File.Open(configPath, FileMode.Create), Encoding.UTF8)
            Dim serializer As New XmlSerializer(GetType(BetterFullscreenConfig))

            serializer.Serialize(stream, config, New XmlSerializerNamespaces({XmlQualifiedName.Empty}))
        End Using
    End Sub

    Public Function GetProfileNames(ByRef config As BetterFullscreenConfig) As List(Of String)
        Return config.Profile.Select(Function(p) p.Name).ToList()
    End Function

    Public Function GetProfile(profileName As String, ByRef config As BetterFullscreenConfig) As Profile
        Return config.Profile.FirstOrDefault(Function(p) p.Name = profileName)
    End Function

    Public Function FindProfile(title As String, [class] As String, ByRef config As BetterFullscreenConfig) As Profile
        Dim profile = config.Profile.FirstOrDefault(Function(p) p.Title = title And p.Class = [class] And p.Enabled = True)
        If profile IsNot Nothing Then
            Return profile
        End If

        ' Titleless Profile
        profile = config.Profile.FirstOrDefault(Function(p) p.Title = "" And Not p.Class = "" And p.Class = [class] And p.Enabled = True)
        If profile IsNot Nothing Then
            Return profile
        End If

        ' Classless profile
        profile = config.Profile.FirstOrDefault(Function(p) p.Class = "" And Not p.Title = "" And p.Title = title And p.Enabled = True)
        If profile IsNot Nothing Then
            Return profile
        End If

        Return Nothing
    End Function

    Public Function GetProfiles(ByRef config As BetterFullscreenConfig) As List(Of Profile)
        Return config.Profile
    End Function

    Public Function GetCurrentProfile(ByRef config As BetterFullscreenConfig) As Profile
        Return config.Profile.FirstOrDefault(Function(p) p.IsCurrentProfile = True)
    End Function

    Public Sub RemoveProfile(profileName As String, ByRef config As BetterFullscreenConfig)
        config.Profile.RemoveAll(Function(p) p.Name = profileName)
        SaveConfig(config.Settings.ConfigPath, config)
    End Sub

    Public Sub AddProfile(profile As Profile, ByRef config As BetterFullscreenConfig)
        config.Profile.Add(profile)
        SaveConfig(config.Settings.ConfigPath, config)
    End Sub

End Module
