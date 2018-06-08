Imports Autofac
Public Class ObjectContainerBuilder
    Implements IObjectContainerBuilder

    Public Sub Register(builder As ContainerBuilder) Implements IObjectContainerBuilder.Register
        builder.RegisterType(Of WebMetricFactory)().As(Of IWebMetricFactory)()
        builder.RegisterType(Of WebMetricSaver)().As(Of IWebMetricSaver)()
    End Sub
End Class
