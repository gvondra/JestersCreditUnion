Imports JestersCreditUnion.DataTier.Core
Imports Autofac
Public Class UnassignedTaskFactory
    Implements IUnassignedTaskFactory

    Private m_container As IContainer

    Public Sub New()
        m_container = ObjectContainer.GetContainer
    End Sub

    Public Function GetByUser(settings As ISettings, userId As Guid) As IEnumerable(Of IUnassignedTask) Implements IUnassignedTaskFactory.GetByUser
        Dim factory As IUnassignedTaskDataFactory
        Dim result As IEnumerable(Of IUnassignedTask)
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of IUnassignedTaskDataFactory)
            result = From d In factory.GetByUser(New Settings(settings), userId)
                     Select New UnassignedTask(d)
        End Using
        Return result
    End Function
End Class
