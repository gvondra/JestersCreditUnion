Imports JestersCreditUnion.DataTier.Core
Imports Autofac
Friend NotInheritable Class ObjectContainer

    Private Shared m_container As IContainer

    Shared Sub New()
        Dim builder As New ContainerBuilder()

        builder.RegisterType(Of GroupDataFactory)().As(Of IGroupDataFactory)()
        builder.RegisterType(Of GroupDataSaver)()

        builder.RegisterType(Of UserDataFactory).As(Of IUserDataFactory)()
        builder.RegisterType(Of UserDataSaver)()
        builder.RegisterType(Of UserAccountDataSaver)()

        builder.RegisterType(Of UserGroupDataFactory).As(Of IUserGroupDataFactory)()
        builder.RegisterType(Of UserGroupDataSaver)()

        builder.RegisterType(Of UserManagementSettings)().As(Of [Interface].UserManagement.ISettings)()
        builder.RegisterType(Of [Interface].UserManagement.UserInfoService)().As(Of [Interface].UserManagement.IUserInfoService)()

        m_container = builder.Build()
    End Sub

    Public Shared Function GetContainer() As IContainer
        Return m_container
    End Function
End Class
