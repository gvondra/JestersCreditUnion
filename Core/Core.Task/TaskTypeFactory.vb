Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class TaskTypeFactory
    Implements ITaskTypeFactory

    Private m_container As IContainer

    Public Sub New()
        m_container = ObjectContainer.GetContainer()
    End Sub

    Public Function Create() As ITaskType Implements ITaskTypeFactory.Create
        Return New TaskType(New TaskTypeData())
    End Function

    Public Function [Get](settings As ISettings, taskTypeId As Guid) As ITaskType Implements ITaskTypeFactory.Get
        Dim data As TaskTypeData
        Dim factory As ITaskTypeDataFactory
        Dim result As TaskType = Nothing
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of ITaskTypeDataFactory)()
            data = factory.Get(New Settings(settings), taskTypeId)
        End Using
        If data IsNot Nothing Then
            result = New TaskType(data)
        End If

        Return result
    End Function

    Public Function GetAll(settings As ISettings) As IEnumerable(Of ITaskType) Implements ITaskTypeFactory.GetAll
        Dim result As IEnumerable(Of ITaskType) = {}
        Dim factory As ITaskTypeDataFactory
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of ITaskTypeDataFactory)()
            result = From data In factory.GetAll(New Settings(settings))
                     Select New TaskType(data)
        End Using
        Return result
    End Function
End Class
