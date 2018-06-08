Public Interface IDataManagedState(Of T)
    Inherits ICloneable
    Inherits IDbTransactionObserver

    Property DataStateManager As IDataStateManager(Of T)

    Sub AcceptChanges()
End Interface
