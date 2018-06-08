Public Class UserGroupDataSaver
    Implements IDataCreator
    Implements IDataUpdater

    Private m_userGroupData As UserGroupData
    Private m_transactionHandler As ITransactionHandler

    Public Sub New(ByVal transactionHandler As ITransactionHandler, ByVal userGroupData As UserGroupData)
        m_transactionHandler = transactionHandler
        m_userGroupData = userGroupData
    End Sub

    Public Sub Create() Implements IDataCreator.Create
        Create(New DbProviderFactory)
    End Sub

    Public Sub Create(ByVal providerFactory As IDbProviderFactory)
        Dim timestamp As IDbDataParameter

        If m_userGroupData.DataStateManager.GetState(m_userGroupData) = IDataStateManager(Of UserData).enumState.New Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_userGroupData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "adp.iUserGroup"

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)

                AddParameter(providerFactory, command.Parameters, "userId", DbType.Guid, GetParameterValue(m_userGroupData.UserId))
                AddParameter(providerFactory, command.Parameters, "groupId", DbType.Guid, GetParameterValue(m_userGroupData.GroupId))
                AddParameter(providerFactory, command.Parameters, "isActive", DbType.Boolean, GetParameterValue(m_userGroupData.IsActive))

                command.ExecuteNonQuery()
                m_userGroupData.CreateTimestamp = CType(timestamp.Value, Date)
                m_userGroupData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub

    Public Sub Update() Implements IDataUpdater.Update
        Update(New DbProviderFactory)
    End Sub

    Public Sub Update(ByVal providerFactory As IDbProviderFactory)
        Dim timestamp As IDbDataParameter

        If m_userGroupData.DataStateManager.GetState(m_userGroupData) = IDataStateManager(Of UserData).enumState.Updated Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_userGroupData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "adp.uUserGroup"

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)

                AddParameter(providerFactory, command.Parameters, "userId", DbType.Guid, GetParameterValue(m_userGroupData.UserId))
                AddParameter(providerFactory, command.Parameters, "groupId", DbType.Guid, GetParameterValue(m_userGroupData.GroupId))
                AddParameter(providerFactory, command.Parameters, "isActive", DbType.Boolean, GetParameterValue(m_userGroupData.IsActive))

                command.ExecuteNonQuery()
                m_userGroupData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub
End Class
