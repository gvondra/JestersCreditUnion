Public Class TaskDataSaver
    Implements IDataCreator
    Implements IDataUpdater

    Private m_taskData As TaskData
    Private m_transactionHandler As ITransactionHandler

    Public Sub New(ByVal transactionHandler As ITransactionHandler, ByVal taskData As TaskData)
        m_transactionHandler = transactionHandler
        m_taskData = taskData
    End Sub

    Public Sub Create() Implements IDataCreator.Create
        Create(New DbProviderFactory)
    End Sub

    Public Sub Create(ByVal providerFactory As IDbProviderFactory)
        Dim id As IDbDataParameter
        Dim timestamp As IDbDataParameter

        If m_taskData.DataStateManager.GetState(m_taskData) = IDataStateManager(Of UserData).enumState.New Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_taskData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "adp.iTask"

                id = CreateParameter(providerFactory, "id", DbType.Guid)
                id.Direction = ParameterDirection.Output
                command.Parameters.Add(id)

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)

                AddParameter(providerFactory, command.Parameters, "taskTypeId", DbType.Guid, GetParameterValue(m_taskData.TaskTypeId))

                CommonParameters(providerFactory, command)

                command.ExecuteNonQuery()
                m_taskData.TaskId = CType(id.Value, Guid)
                m_taskData.CreateTimestamp = CType(timestamp.Value, Date)
                m_taskData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub

    Public Sub Update() Implements IDataUpdater.Update
        Update(New DbProviderFactory())
    End Sub

    Public Sub Update(ByVal providerFactory As IDbProviderFactory)
        Dim timestamp As IDbDataParameter

        If m_taskData.DataStateManager.GetState(m_taskData) = IDataStateManager(Of UserData).enumState.Updated Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_taskData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "adp.uTask"

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)


                AddParameter(providerFactory, command.Parameters, "id", DbType.Guid, m_taskData.TaskId)

                CommonParameters(providerFactory, command)

                command.ExecuteNonQuery()
                m_taskData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub

    Private Sub CommonParameters(ByVal providerFactory As IDbProviderFactory, ByVal command As IDbCommand)
        AddParameter(providerFactory, command.Parameters, "userId", DbType.Guid, GetParameterValue(m_taskData.UserId))
        AddParameter(providerFactory, command.Parameters, "message", DbType.String, GetParameterValue(m_taskData.Message))
        AddParameter(providerFactory, command.Parameters, "isClosed", DbType.Boolean, GetParameterValue(m_taskData.IsClosed))
    End Sub
End Class
