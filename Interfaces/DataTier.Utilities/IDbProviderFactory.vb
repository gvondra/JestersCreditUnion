Public Interface IDbProviderFactory
    Function CreateConnection() As IDbConnection
    Function CreateParameter() As IDbDataParameter
    Function OpenConnection(ByVal connectionString As String) As IDbConnection
    Sub EstablishTransaction(ByVal transactionHandler As ITransactionHandler)
    Sub EstablishTransaction(ByVal transactionHandler As ITransactionHandler, ByVal observer As IDbTransactionObserver)
End Interface
