Public Interface IUserFactory
    Function Create() As IUser
    Function GetBySubscriberId(ByVal settings As ISettings, ByVal subscriberId As String) As IUser
    Function GetByEmailAddress(ByVal settings As ISettings, ByVal emailAddress As String) As IUser
    Function [Get](ByVal settings As ISettings, ByVal userId As Guid) As IUser
    Function Search(ByVal settings As ISettings, ByVal searchText As String) As IEnumerable(Of IUser)
End Interface
