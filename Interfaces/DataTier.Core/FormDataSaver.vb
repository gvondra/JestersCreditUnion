Imports System.Xml
Public Class FormDataSaver
    Implements IDataCreator

    Private m_formData As FormData
    Private m_transactionHandler As ITransactionHandler

    Public Sub New(ByVal transactionHandler As ITransactionHandler, ByVal formData As FormData)
        m_transactionHandler = transactionHandler
        m_formData = formData
    End Sub

    Public Sub Create() Implements IDataCreator.Create
        Create(New DbProviderFactory)
    End Sub

    Public Sub Create(ByVal providerFactory As IDbProviderFactory)
        Dim id As IDbDataParameter
        Dim timestamp As IDbDataParameter

        If m_formData.DataStateManager.GetState(m_formData) = IDataStateManager(Of UserData).enumState.New Then
            providerFactory.EstablishTransaction(m_transactionHandler, m_formData)
            Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
                command.Transaction = m_transactionHandler.Transaction.InnerTransaction
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "adp.iForm"

                id = CreateParameter(providerFactory, "id", DbType.Guid)
                id.Direction = ParameterDirection.Output
                command.Parameters.Add(id)

                timestamp = CreateParameter(providerFactory, "timestamp", DbType.DateTime)
                timestamp.Direction = ParameterDirection.Output
                command.Parameters.Add(timestamp)

                AddParameter(providerFactory, command.Parameters, "userId", DbType.Guid, GetParameterValue(m_formData.UserId))
                AddParameter(providerFactory, command.Parameters, "formTypeId", DbType.Int16, GetParameterValue(m_formData.FormTypeId))
                AddParameter(providerFactory, command.Parameters, "style", DbType.Int16, GetParameterValue(m_formData.Style))
                AddParameter(providerFactory, command.Parameters, "content", DbType.Xml, GetParameterValue(m_formData.Content))

                command.ExecuteNonQuery()
                m_formData.FormId = CType(id.Value, Guid)
                m_formData.CreateTimestamp = CType(timestamp.Value, Date)
                m_formData.UpdateTimestamp = CType(timestamp.Value, Date)
            End Using
        End If
    End Sub
End Class
