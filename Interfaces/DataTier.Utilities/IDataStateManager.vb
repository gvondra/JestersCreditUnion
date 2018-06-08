Public Interface IDataStateManager(Of T)
    Enum enumState As Int16
        [New] = 0
        Updated = 1
        Unchaged = 2
    End Enum

    Property Original As T
    Function GetState(ByVal target As T) As enumState
End Interface
