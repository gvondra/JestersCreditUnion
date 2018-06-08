Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class EventTypeFactory
    Implements IEventTypeFactory

    Private m_container As IContainer
    Private m_eventTypeSaver As IEventTypeSaver
    Private m_taskTypeFactory As ITaskTypeFactory

    Public Sub New(ByVal eventTypeSaver As IEventTypeSaver, ByVal taskTypeFactory As ITaskTypeFactory)
        m_container = ObjectContainer.GetContainer
        m_eventTypeSaver = eventTypeSaver
        m_taskTypeFactory = taskTypeFactory
    End Sub

    Public Sub New(ByVal container As IContainer, ByVal eventTypeSaver As IEventTypeSaver)
        m_container = container
        m_eventTypeSaver = eventTypeSaver
    End Sub

    Public Function [Get](settings As ISettings, type As enumEventType) As IEventType Implements IEventTypeFactory.Get
        Dim data As EventTypeData
        Dim dataFactory As IEventTypeDataFactory
        Dim blnSave As Boolean = False
        Dim result As EventType

        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            dataFactory = scope.Resolve(Of IEventTypeDataFactory)()
            data = dataFactory.Get(New Settings(settings), CType(type, Int16))
            If data Is Nothing Then
                data = New EventTypeData() With {.EventTypeId = CType(type, Int16), .Title = type.ToString}
                blnSave = True
            End If
        End Using
        result = New EventType(data, m_taskTypeFactory)

        If blnSave Then
            m_eventTypeSaver.Create(settings, result)
        End If
        Return result
    End Function

    Public Function GetAll(settings As ISettings) As IEnumerable(Of IEventType) Implements IEventTypeFactory.GetAll
        Dim dataList As IEnumerable(Of EventTypeData)
        Dim types As IEnumerable(Of IEventType) = {}
        Dim factory As IEventTypeDataFactory
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of IEventTypeDataFactory)()
            dataList = factory.GetAll(New Settings(settings))
            types = From i In [Enum].GetValues(GetType(enumEventType))
                    Where CType(i, enumEventType) <> enumEventType.NotSet
                    Select GetEventType(CType(i, enumEventType), dataList)

        End Using
        Return types
    End Function

    Private Function GetEventType(ByVal i As enumEventType, ByVal dataList As IEnumerable(Of EventTypeData)) As IEventType
        Dim data As EventTypeData = dataList.FirstOrDefault(Function(d As EventTypeData) CType(d.EventTypeId, enumEventType) = i)

        If data Is Nothing Then
            data = New EventTypeData() With {.EventTypeId = CType(i, Short), .Title = i.ToString}
        End If
        Return New EventType(data, m_taskTypeFactory)
    End Function
End Class
