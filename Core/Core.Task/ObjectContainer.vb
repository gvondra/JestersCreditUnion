Imports JestersCreditUnion.DataTier.Core
Imports Autofac
Friend NotInheritable Class ObjectContainer

    Private Shared m_container As IContainer

    Shared Sub New()
        Dim builder As New ContainerBuilder()

        builder.RegisterType(Of TaskDataFactory)().As(Of ITaskDataFactory)()
        builder.RegisterType(Of TaskDataSaver)()

        builder.RegisterType(Of TaskTypeGroupDataFactory)().As(Of ITaskTypeGroupDataFactory)()
        builder.RegisterType(Of TaskTypeGroupDataSaver)()

        builder.RegisterType(Of EventTypeTaskTypeDataFactory)().As(Of IEventTypeTaskTypeDataFactory)()
        builder.RegisterType(Of EventTypeTaskTypeDataSaver)()

        builder.RegisterType(Of TaskTypeDataFactory)().As(Of ITaskTypeDataFactory)()
        builder.RegisterType(Of TaskTypeDataSaver)()

        builder.RegisterType(Of UnassignedTaskDataFactory)().As(Of IUnassignedTaskDataFactory)()

        m_container = builder.Build()
    End Sub

    Public Shared Function GetContainer() As IContainer
        Return m_container
    End Function
End Class
