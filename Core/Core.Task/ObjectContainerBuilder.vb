Imports Autofac
Public Class ObjectContainerBuilder
    Implements IObjectContainerBuilder

    Public Sub Register(builder As ContainerBuilder) Implements IObjectContainerBuilder.Register
        builder.RegisterType(Of TaskTypeFactory)().As(Of ITaskTypeFactory)()
        builder.RegisterType(Of TaskTypeSaver)().As(Of ITaskTypeSaver)()
        builder.RegisterType(Of TaskFactory)().As(Of ITaskFactory)()
        builder.RegisterType(Of UnassignedTaskFactory)().As(Of IUnassignedTaskFactory)()
        builder.RegisterType(Of TaskSaver)().As(Of ITaskSaver)()
        builder.RegisterType(Of TaskTypeEventTypeSaver)().As(Of ITaskTypeEventTypeSaver)()
        builder.RegisterType(Of TaskTypeGroupSaver)().As(Of ITaskTypeGroupSaver)()
    End Sub
End Class
