Imports JestersCreditUnion.DataTier.Core
Imports Autofac
Friend NotInheritable Class ObjectContainer

    Private Shared m_container As IContainer

    Shared Sub New()
        Dim builder As New ContainerBuilder()

        builder.RegisterType(Of WebMetricDataFactory)().As(Of IWebMetricDataFactory)()
        builder.RegisterType(Of WebMetricDataSaver)()
        builder.RegisterType(Of WebMetricAttributeDataSaver)()

        m_container = builder.Build()
    End Sub

    Public Shared Function GetContainer() As IContainer
        Return m_container
    End Function
End Class
