Public Interface IGroupFactory
    Function Create() As IGroup
    Function [Get](ByVal settings As ISettings, ByVal groupId As Guid) As IGroup
    Function GetAll(ByVal settings As ISettings) As IEnumerable(Of IGroup)
End Interface
