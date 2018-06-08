Public Class EventTaskDataSaver
    Implements IDataCreator

    Private m_eventTaskData As EventTaskData
    Private m_transactionHandler As ITransactionHandler

    Public Sub New(ByVal transactionHandler As ITransactionHandler, ByVal eventTaskData As EventTaskData)
        m_transactionHandler = transactionHandler
        m_eventTaskData = eventTaskData
    End Sub

    Public Sub Create() Implements IDataCreator.Create
        Create(New DbProviderFactory)
    End Sub

    Public Sub Create(ByVal providerFactory As IDbProviderFactory)
        Dim timestamp As IDbDataParameter

        If m_eventTaskData.DataStateManager.GetState(m_eventTaskData) = IDataStateManager(Of UserData).enumState.New Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_eventTaskData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "adp.iEventTask"

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)

                AddParameter(providerFactory, command.Parameters, "eventId", DbType.Guid, GetParameterValue(m_eventTaskData.EventId))
                AddParameter(providerFactory, command.Parameters, "taskId", DbType.Guid, GetParameterValue(m_eventTaskData.TaskId))

                command.ExecuteNonQuery()
                m_eventTaskData.CreateTimestamp = CType(timestamp.Value, Date)
                m_eventTaskData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub
End Class
