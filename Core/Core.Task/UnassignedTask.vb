Imports JestersCreditUnion.DataTier.Core.Models
Public Class UnassignedTask
    Implements IUnassignedTask

    Private m_unassignedTaskData As UnassignedTaskData

    Friend Sub New(ByVal unassignedTaskData As UnassignedTaskData)
        m_unassignedTaskData = unassignedTaskData
    End Sub

    Public ReadOnly Property CreateTimestamp As DateTime Implements IUnassignedTask.CreateTimestamp
        Get
            Return m_unassignedTaskData.CreateTimestamp
        End Get
    End Property

    Public ReadOnly Property GroupId As Guid? Implements IUnassignedTask.GroupId
        Get
            Return m_unassignedTaskData.GroupId
        End Get
    End Property

    Public ReadOnly Property GroupName As String Implements IUnassignedTask.GroupName
        Get
            Return m_unassignedTaskData.GroupName
        End Get
    End Property

    Public ReadOnly Property Message As String Implements IUnassignedTask.Message
        Get
            Return m_unassignedTaskData.Message
        End Get
    End Property

    Public ReadOnly Property TaskId As Guid Implements IUnassignedTask.TaskId
        Get
            Return m_unassignedTaskData.TaskId
        End Get
    End Property

    Public ReadOnly Property TaskTypeId As Guid Implements IUnassignedTask.TaskTypeId
        Get
            Return m_unassignedTaskData.TaskTypeId
        End Get
    End Property

    Public ReadOnly Property TaskTypeTitle As String Implements IUnassignedTask.TaskTypeTitle
        Get
            Return m_unassignedTaskData.TaskTypeTitle
        End Get
    End Property

    Public ReadOnly Property UpdateTimestamp As DateTime Implements IUnassignedTask.UpdateTimestamp
        Get
            Return m_unassignedTaskData.UpdateTimestamp
        End Get
    End Property
End Class
