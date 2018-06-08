Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class TaskFactory
    Implements ITaskFactory

    Private m_taskTypeFactory As ITaskTypeFactory
    Private m_userFactory As IUserFactory
    Private m_container As IContainer

    Public Sub New(ByVal userFactory As IUserFactory, ByVal taskTypeFactory As ITaskTypeFactory)
        m_userFactory = userFactory
        m_taskTypeFactory = taskTypeFactory
        m_container = ObjectContainer.GetContainer
    End Sub

    Public Function Create(taskType As ITaskType) As ITask Implements ITaskFactory.Create
        Return New Task(m_userFactory, m_taskTypeFactory, taskType)
    End Function

    Public Function [Get](settings As ISettings, taskId As Guid) As ITask Implements ITaskFactory.Get
        Dim factory As ITaskDataFactory
        Dim data As TaskData
        Dim result As Task = Nothing
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of ITaskDataFactory)()
            data = factory.Get(New Settings(settings), taskId)
            If data IsNot Nothing Then
                result = New Task(m_userFactory, m_taskTypeFactory, data)
            End If
        End Using
        Return result
    End Function

    Public Function GetByUserId(settings As ISettings, userId As Guid) As IEnumerable(Of ITask) Implements ITaskFactory.GetByUserId
        Dim factory As ITaskDataFactory
        Dim result As IEnumerable(Of Task)
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of ITaskDataFactory)()
            result = From d In factory.GetByUserId(New Settings(settings), userId)
                     Select New Task(m_userFactory, m_taskTypeFactory, d)
        End Using
        Return result
    End Function

    Public Function GetFormIds(settings As ISettings, taskId As Guid) As IEnumerable(Of Guid) Implements ITaskFactory.GetFormIds
        Dim factory As ITaskDataFactory
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of ITaskDataFactory)()
            Return factory.GetFormIds(New Settings(settings), taskId)
        End Using
    End Function
End Class
