Public Class UserDataSaver
    Implements IDataCreator
    Implements IDataUpdater

    Private m_userData As UserData
    Private m_transactionHandler As ITransactionHandler

    Public Sub New(ByVal transactionHandler As ITransactionHandler, ByVal userData As UserData)
        m_transactionHandler = transactionHandler
        m_userData = userData
    End Sub

    Public Sub Create() Implements IDataCreator.Create
        Create(New DbProviderFactory)
    End Sub

    Public Sub Create(ByVal providerFactory As IDbProviderFactory)
        Dim id As IDbDataParameter
        Dim timestamp As IDbDataParameter

        If m_userData.DataStateManager.GetState(m_userData) = IDataStateManager(Of UserData).enumState.New Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_userData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "jcu.iUser"

                id = CreateParameter(providerFactory, "id", DbType.Guid)
                id.Direction = ParameterDirection.Output
                command.Parameters.Add(id)

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)

                CommonParameters(providerFactory, command)

                command.ExecuteNonQuery()
                m_userData.UserId = CType(id.Value, Guid)
                m_userData.CreateTimestamp = CType(timestamp.Value, Date)
                m_userData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub

    Public Sub Update() Implements IDataUpdater.Update
        Update(New DbProviderFactory())
    End Sub

    Public Sub Update(ByVal providerFactory As IDbProviderFactory)
        Dim timestamp As IDbDataParameter

        If m_userData.DataStateManager.GetState(m_userData) = IDataStateManager(Of UserData).enumState.Updated Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_userData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "jcu.uUser"

                AddParameter(providerFactory, command.Parameters, "id", DbType.Guid, m_userData.UserId)

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)

                CommonParameters(providerFactory, command)

                command.ExecuteNonQuery()
                m_userData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub

    Private Sub CommonParameters(ByVal providerFactory As IDbProviderFactory, ByVal command As IDbCommand)
        AddParameter(providerFactory, command.Parameters, "fullName", DbType.String, GetParameterValue(m_userData.FullName))
        AddParameter(providerFactory, command.Parameters, "shortName", DbType.String, GetParameterValue(m_userData.ShortName))
        AddParameter(providerFactory, command.Parameters, "birthDate", DbType.Date, GetParameterValue(m_userData.BirthDate))
        AddParameter(providerFactory, command.Parameters, "emailAddress", DbType.String, GetParameterValue(m_userData.EmailAddress))
        AddParameter(providerFactory, command.Parameters, "phoneNumber", DbType.AnsiString, GetParameterValue(m_userData.PhoneNumber))
    End Sub
End Class
