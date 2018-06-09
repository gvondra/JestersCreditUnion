<Flags()> Public Enum enumRole As UInt16
    None = 0
    UserAdministrator = &H1
    TaskAdministrator = &H2
    FormAdminstrator = &H4
    TaskProcessor = &H8
    SuperUser = &HFF
End Enum

Public Enum enumFormType As Int16
    NotSet = 0
    RoleRequest = 1
End Enum

Public Enum enumFormStyle As Int16
    NotSet = 0
    RoleRequest = 1
End Enum

Public Enum enumEventType As Int16
    NotSet = 0
    RoleRequest = 1
End Enum