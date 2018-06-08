Public Interface ITaskTypeEventType
    Inherits IEventType
    Inherits ISavable

    Property IsActive As Boolean

    Sub Save(ByVal transactionHandler As ITransactionHandler)
End Interface
