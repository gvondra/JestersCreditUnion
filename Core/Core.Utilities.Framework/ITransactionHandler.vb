Public Interface ITransactionHandler
    Inherits ISettings

    Property DbConnection As IDbConnection
    Property DbTransaction As JestersCreditUnion.DataTier.Utilities.IDbTransaction
End Interface
