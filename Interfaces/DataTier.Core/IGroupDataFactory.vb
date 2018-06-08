Public Interface IGroupDataFactory
    Function [Get](ByVal settings As ISettings, ByVal groupId As Guid) As GroupData
    Function GetAll(ByVal settings As ISettings) As IEnumerable(Of GroupData)
End Interface
