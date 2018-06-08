Imports JestersCreditUnion.DataTier.Core.Models
Public Class Group
    Implements IGroup

    Private m_groupData As GroupData

    Friend Sub New(ByVal groupData As GroupData)
        m_groupData = groupData
    End Sub

    Public ReadOnly Property GroupId As Guid Implements IGroup.GroupId
        Get
            Return m_groupData.GroupId
        End Get
    End Property

    Public Property Name As String Implements IGroup.Name
        Get
            Return m_groupData.Name
        End Get
        Set(value As String)
            m_groupData.Name = value
        End Set
    End Property

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Throw New NotImplementedException()
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Throw New NotImplementedException()
    End Sub
End Class
