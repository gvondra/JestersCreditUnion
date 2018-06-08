Public Class EventTypeDataSaver
    Implements IDataCreator
    Implements IDataUpdater

    Private m_eventTypeData As EventTypeData
    Private m_transactionHandler As ITransactionHandler

    Public Sub New(ByVal transactionHandler As ITransactionHandler, ByVal eventTypeData As EventTypeData)
        m_transactionHandler = transactionHandler
        m_eventTypeData = eventTypeData
    End Sub

    Public Sub Create() Implements IDataCreator.Create
        Create(New DbProviderFactory)
    End Sub

    Public Sub Create(ByVal providerFactory As IDbProviderFactory)
        Dim timestamp As IDbDataParameter

        If m_eventTypeData.DataStateManager.GetState(m_eventTypeData) = IDataStateManager(Of UserData).enumState.New Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_eventTypeData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "jcu.iEventType"

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)

                AddParameter(providerFactory, command.Parameters, "id", DbType.Int16, GetParameterValue(m_eventTypeData.EventTypeId))
                AddParameter(providerFactory, command.Parameters, "title", DbType.String, GetParameterValue(m_eventTypeData.Title))

                command.ExecuteNonQuery()
                m_eventTypeData.CreateTimestamp = CType(timestamp.Value, Date)
                m_eventTypeData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub

    Public Sub Update() Implements IDataUpdater.Update
        Update(New DbProviderFactory)
    End Sub

    Public Sub Update(ByVal providerFactory As IDbProviderFactory)
        Dim timestamp As IDbDataParameter

        If m_eventTypeData.DataStateManager.GetState(m_eventTypeData) = IDataStateManager(Of UserData).enumState.Updated Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_eventTypeData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "jcu.uEventType"

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)

                AddParameter(providerFactory, command.Parameters, "id", DbType.Int16, GetParameterValue(m_eventTypeData.EventTypeId))
                AddParameter(providerFactory, command.Parameters, "title", DbType.String, GetParameterValue(m_eventTypeData.Title))

                command.ExecuteNonQuery()
                m_eventTypeData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub
End Class
