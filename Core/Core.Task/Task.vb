Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class Task
    Implements ITask

    Private m_taskData As TaskData
    Private m_taskType As ITaskType
    Private m_container As IContainer
    Private m_owner As IUser
    Private m_userFactory As IUserFactory
    Private m_taskTypeFactory As ITaskTypeFactory

    Friend Sub New(ByVal userFactory As IUserFactory, ByVal taskTypeFactory As ITaskTypeFactory, ByVal taskData As TaskData)
        m_userFactory = userFactory
        m_taskTypeFactory = taskTypeFactory
        m_taskData = taskData
        m_container = ObjectContainer.GetContainer()
    End Sub

    Friend Sub New(ByVal userFactory As IUserFactory, ByVal taskTypeFactory As ITaskTypeFactory, ByVal taskType As ITaskType)
        m_userFactory = userFactory
        m_taskTypeFactory = taskTypeFactory
        m_taskData = New TaskData
        m_taskType = taskType
        m_container = ObjectContainer.GetContainer()
    End Sub

    Public ReadOnly Property TaskId As Guid Implements ITask.TaskId
        Get
            Return m_taskData.TaskId
        End Get
    End Property

    Public Property Message As String Implements ITask.Message
        Get
            Return m_taskData.Message
        End Get
        Set(value As String)
            m_taskData.Message = value
        End Set
    End Property

    Public Property IsClosed As Boolean Implements ITask.IsClosed
        Get
            Return m_taskData.IsClosed
        End Get
        Set(value As Boolean)
            m_taskData.IsClosed = value
        End Set
    End Property

    Public ReadOnly Property CreateTimestamp As Date Implements ITask.CreateTimestamp
        Get
            Return m_taskData.CreateTimestamp
        End Get
    End Property

    Private Property UserId As Guid?
        Get
            Return m_taskData.UserId
        End Get
        Set(value As Guid?)
            m_taskData.UserId = value
        End Set
    End Property

    Private Property TaskTypeId As Guid
        Get
            Return m_taskData.TaskTypeId
        End Get
        Set(value As Guid)
            m_taskData.TaskTypeId = value
        End Set
    End Property

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            TaskTypeId = m_taskType.TaskTypeId
            If m_owner IsNot Nothing Then
                UserId = m_owner.UserId
            End If
            creator = scope.Resolve(Of TaskDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(TaskData), m_taskData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Dim updater As DataTier.Utilities.IDataUpdater
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            If m_owner IsNot Nothing Then
                UserId = m_owner.UserId
            End If
            updater = scope.Resolve(Of TaskDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(TaskData), m_taskData)
            )
            updater.Update()
        End Using
    End Sub

    Public Function GetUser(settings As ISettings) As IUser Implements ITask.GetUser
        If m_owner Is Nothing AndAlso UserId.HasValue AndAlso UserId.Value.Equals(Guid.Empty) = False Then
            m_owner = m_userFactory.Get(settings, UserId.Value)
        End If

        Return m_owner
    End Function

    Public Sub SetUser(user As IUser) Implements ITask.SetUser
        m_owner = user
    End Sub

    Public Function GetTaskType(settings As ISettings) As ITaskType Implements ITask.GetTaskType
        If m_taskType Is Nothing Then
            m_taskType = m_taskTypeFactory.Get(settings, TaskTypeId)
        End If
        Return m_taskType
    End Function
End Class
