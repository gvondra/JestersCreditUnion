Public Interface ITaskTypeGroup
    Inherits IGroup
    Inherits ISavable

    Property IsActive As Boolean

    Sub Save(ByVal transactionHandler As ITransactionHandler)
End Interface
