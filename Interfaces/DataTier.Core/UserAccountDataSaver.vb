Public Class UserAccountDataSaver
    Implements IDataCreator

    Private m_userAccountData As UserAccountData
    Private m_transactionHandler As ITransactionHandler

    Public Sub New(ByVal transactionHandler As ITransactionHandler, ByVal userAccountData As UserAccountData)
        m_transactionHandler = transactionHandler
        m_userAccountData = userAccountData
    End Sub

    Public Sub Create() Implements IDataCreator.Create
        Create(New DbProviderFactory())
    End Sub

    Public Sub Create(ByVal providerFactory As IDbProviderFactory)
        Dim id As IDbDataParameter
        Dim timestamp As IDbDataParameter

        providerFactory.EstablishTransaction(m_transactionHandler)
        Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
            command.Transaction = m_transactionHandler.Transaction.InnerTransaction
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "jcu.iUserAccount"

            id = CreateParameter(providerFactory, "id", DbType.Guid)
            id.Direction = ParameterDirection.Output
            command.Parameters.Add(id)

            timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
            timestamp.Direction = ParameterDirection.Output
            command.Parameters.Add(timestamp)

            AddParameter(providerFactory, command.Parameters, "userId", DbType.Guid, GetParameterValue(m_userAccountData.UserId))
            AddParameter(providerFactory, command.Parameters, "subscriberId", DbType.String, GetParameterValue(m_userAccountData.SubscriberId))

            command.ExecuteNonQuery()
            m_userAccountData.UserAccountId = CType(id.Value, Guid)
            m_userAccountData.CreateTimestamp = CType(timestamp.Value, Date)
            m_userAccountData.UpdateTimestamp = CType(timestamp.Value, Date)
        End Using
    End Sub
End Class
