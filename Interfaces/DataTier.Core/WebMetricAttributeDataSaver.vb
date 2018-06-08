Public Class WebMetricAttributeDataSaver
    Implements IDataCreator

    Private m_transactionHandler As ITransactionHandler
    Private m_webMetricAttributeData As WebMetricAttributeData

    Public Sub New(ByVal transactionHandler As ITransactionHandler, ByVal webMetricAttributeData As WebMetricAttributeData)
        m_transactionHandler = transactionHandler
        m_webMetricAttributeData = webMetricAttributeData
    End Sub

    Public Sub Create() Implements IDataCreator.Create
        Create(New DbProviderFactory())
    End Sub

    Public Sub Create(ByVal providerFactory As IDbProviderFactory)
        Dim id As IDbDataParameter

        providerFactory.EstablishTransaction(m_transactionHandler)
        Using command As IDbCommand = m_transactionHandler.Connection.CreateCommand
            command.Transaction = m_transactionHandler.Transaction.InnerTransaction
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "jcu.iWebMetricAttribute"

            id = CreateParameter(providerFactory, "id", DbType.Int32)
            id.Direction = ParameterDirection.Output
            command.Parameters.Add(id)

            AddParameter(providerFactory, command.Parameters, "WebMetricId", DbType.Int32, GetParameterValue(m_webMetricAttributeData.WebMetricId))
            AddParameter(providerFactory, command.Parameters, "Key", DbType.String, GetParameterValue(m_webMetricAttributeData.Key))
            AddParameter(providerFactory, command.Parameters, "Value", DbType.String, GetParameterValue(m_webMetricAttributeData.Value))

            command.ExecuteNonQuery()
            m_webMetricAttributeData.WebMetricAttributeId = CType(id.Value, Integer)
        End Using
    End Sub
End Class
