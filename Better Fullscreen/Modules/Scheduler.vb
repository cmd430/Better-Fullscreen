Imports System.IO
Imports System.Reflection
Imports System.Security.Principal
Imports Microsoft.Win32.TaskScheduler

Public Class Scheduler
    Private Name As String
    Private Author As String
    Private Description As String
    Private FilePath As String
    Private Arguments As String
    Private Enabled As Boolean

    Public Sub New(TaskName As String, ByVal TaskAuthor As String, ByVal TaskDescription As String, ByVal TaskFile As String, ByVal TaskArguments As String, ByVal TaskEnabled As Boolean)
        Name = TaskName
        Author = TaskAuthor
        Description = TaskDescription
        FilePath = TaskFile
        Arguments = TaskArguments
        Enabled = TaskEnabled
    End Sub

    Public Sub AddTask()
        Dim ts = New TaskService()
        Dim td = ts.NewTask()
        td.RegistrationInfo.Author = Author
        td.RegistrationInfo.Description = Description
        td.Triggers.Add(New LogonTrigger With {
            .UserId = WindowsIdentity.GetCurrent().Name,
            .Enabled = True
        })
        td.Settings.AllowHardTerminate = True
        td.Settings.StartWhenAvailable = True
        td.Settings.RestartInterval = TimeSpan.FromMinutes(1)
        td.Settings.RestartCount = 3
        td.Settings.AllowDemandStart = True
        td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew
        td.Settings.Compatibility = TaskCompatibility.V2_3
        td.Settings.ExecutionTimeLimit = TimeSpan.Zero
        td.Settings.DisallowStartIfOnBatteries = False
        td.Principal.RunLevel = TaskRunLevel.Highest
        td.Principal.LogonType = TaskLogonType.InteractiveToken
        td.Settings.Enabled = Enabled

        Dim action = New ExecAction(Assembly.GetExecutingAssembly().Location, Nothing, Nothing)

        If FilePath <> String.Empty AndAlso File.Exists(FilePath) Then
            action = New ExecAction(FilePath, Arguments)
        End If

        td.Actions.Add(action)
        ts.RootFolder.RegisterTaskDefinition(Name, td)
    End Sub

    Public Sub UpdateTask()
        Dim ts = New TaskService()
        Dim task = ts.RootFolder.GetTasks().Where(Function(a) a.Name.ToLower() = Name.ToLower()).FirstOrDefault()
        If task IsNot Nothing Then
            Dim action = New ExecAction(Assembly.GetExecutingAssembly().Location, Nothing, Nothing)
            If FilePath <> String.Empty AndAlso File.Exists(FilePath) Then
                action = New ExecAction(FilePath, Arguments)
            End If
            task.Definition.Actions.RemoveAt(0)
            task.Definition.Actions.Add(action)
            task.RegisterChanges()
        End If
    End Sub

    Public Sub ToggleTask()
        Dim ts = New TaskService()
        Dim task = ts.RootFolder.GetTasks().Where(Function(a) a.Name.ToLower() = Name.ToLower()).FirstOrDefault()
        If task IsNot Nothing Then
            task.Enabled = Not task.Enabled
        End If
    End Sub

    Public Sub DeleteTask()
        Dim ts = New TaskService()
        Dim task = ts.RootFolder.GetTasks().Where(Function(a) a.Name.ToLower() = Name.ToLower()).FirstOrDefault()
        If task IsNot Nothing Then
            ts.RootFolder.DeleteTask(Name)
        End If
    End Sub

    Public Function GetTask() As Task
        Dim ts = New TaskService()
        Dim task = ts.RootFolder.GetTasks().Where(Function(a) a.Name.ToLower() = Name.ToLower()).FirstOrDefault()
        Return task
    End Function

End Class

