Imports Autofac
Public Class ObjectContainerBuilder
    Implements IObjectContainerBuilder

    Public Sub Register(builder As ContainerBuilder) Implements IObjectContainerBuilder.Register
        builder.RegisterType(Of EventTypeFactory)().As(Of IEventTypeFactory)()
        builder.RegisterType(Of EventTypeSaver)().As(Of IEventTypeSaver)()
        builder.RegisterType(Of EventFactory)().As(Of IEventFactory)()
        builder.RegisterType(Of EventSaver)().As(Of IEventSaver)()
        builder.RegisterType(Of EventTriggerFactory)().As(Of IEventTriggerFactory)()
    End Sub
End Class
