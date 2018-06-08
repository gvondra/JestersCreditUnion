Public Interface IUserDataFactory
    Function GetBySubscriberId(ByVal settings As ISettings, ByVal subscriberId As String) As UserData
    Function GetByEmailAddress(ByVal settings As ISettings, ByVal emailAddress As String) As IEnumerable(Of UserData)
    Function [Get](ByVal settings As ISettings, ByVal userId As Guid) As UserData
    Function Search(ByVal settings As ISettings, ByVal searchText As String) As IEnumerable(Of UserData)
End Interface
