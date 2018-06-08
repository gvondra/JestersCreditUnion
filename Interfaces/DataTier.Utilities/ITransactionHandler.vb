Public Interface ITransactionHandler
    Inherits ISettings

    Property Connection As IDbConnection
    Property Transaction As IDbTransaction
End Interface
