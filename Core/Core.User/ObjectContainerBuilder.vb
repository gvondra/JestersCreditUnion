Imports Autofac
Public Class ObjectContainerBuilder
    Implements IObjectContainerBuilder

    Public Sub Register(builder As ContainerBuilder) Implements IObjectContainerBuilder.Register
        builder.RegisterType(Of UserFactory)().As(Of IUserFactory)()
        builder.RegisterType(Of UserSaver)().As(Of IUserSaver)()
        builder.RegisterType(Of UserUpdater)().As(Of IUserUpdater)()
        builder.RegisterType(Of GroupFactory)().As(Of IGroupFactory)()
        builder.RegisterType(Of GroupSaver)().As(Of IGroupSaver)()
        builder.RegisterType(Of UserGroupSaver)().As(Of IUserGroupSaver)()
    End Sub
End Class
