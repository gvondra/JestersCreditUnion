Public Interface IUserGroupDataFactory
    Function GetByUserId(ByVal settings As ISettings, ByVal userId As Guid) As IEnumerable(Of UserGroupData)
End Interface
