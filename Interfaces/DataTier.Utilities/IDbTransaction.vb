Public Interface IDbTransaction
    Inherits System.Data.IDbTransaction

    ReadOnly Property InnerTransaction As System.Data.IDbTransaction

    Sub AddObserver(ByVal observer As IDbTransactionObserver)
End Interface
