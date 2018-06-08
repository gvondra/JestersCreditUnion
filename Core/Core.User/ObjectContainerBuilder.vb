Imports Autofac
Public Class ObjectContainerBuilder
    Implements IObjectContainerBuilder

    Public Sub Register(builder As ContainerBuilder) Implements IObjectContainerBuilder.Register
        builder.RegisterType(Of UserFactory)().As(Of IUserFactory)()
        builder.RegisterType(Of UserSaver)().As(Of IUserSaver)()
    End Sub
End Class
