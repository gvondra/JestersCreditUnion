Imports Autofac
Public Class ObjectContainer

    Private Shared m_container As IContainer

    Shared Sub New()
        Dim builder As New ContainerBuilder()

        builder.RegisterType(Of TokenFactory)().As(Of ITokenFactory)()
        builder.RegisterType(Of JestersCreditUnion.Service.Common.UserFactory)() _
            .As(Of JestersCreditUnion.Service.Common.IUserFactory)()


        builder.RegisterType(Of JestersCreditUnion.Interface.AbyssalDataProcessor.UserFactory)() _
            .As(Of JestersCreditUnion.Interface.AbyssalDataProcessor.IUserFactory)()


        builder.RegisterType(Of JestersCreditUnion.Interface.UserManagement.UserInfoService)() _
            .As(Of JestersCreditUnion.Interface.UserManagement.IUserInfoService)()

        m_container = builder.Build()
    End Sub

    Public Shared Function GetContainer() As IContainer
        Return m_container
    End Function
End Class
