Public Interface IUserGroupSaver
    Sub Save(ByVal settings As ISettings, ByVal userGroups As IEnumerable(Of IUserGroup))
End Interface
