Imports Autofac
Public Class ObjectContainerBuilder
    Implements IObjectContainerBuilder

    Public Sub Register(builder As ContainerBuilder) Implements IObjectContainerBuilder.Register
        builder.RegisterType(Of FormFactory)().As(Of IFormFactory)()
        builder.RegisterType(Of FormSaver)().As(Of IFormSaver)()
        builder.RegisterType(Of FormSerializerFactory)().As(Of IFormSerializerFactory)()
        builder.RegisterType(Of FormContentTransformFactory)().As(Of IFormContentTransormFactory)()
    End Sub
End Class
