Imports Autofac
Imports AutoMapper
Imports System.Net
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class EventTypeController
        Inherits ControllerBase

        Private Shared m_mapperConfiguration As MapperConfiguration

        Shared Sub New()
            m_mapperConfiguration = New MapperConfiguration(Sub(exp As IMapperConfigurationExpression)
                                                                exp.CreateMap(Of IEventType, EventType)()
                                                                exp.CreateMap(Of EventType, IEventType)()
                                                            End Sub)
        End Sub

        'todo add error handling
        <HttpGet(), ClaimsAuthorization(ClaimTypes:="TA")> Public Shadows Function GetEventType(ByVal id As Short) As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim innerType As IEventType
            Dim typeFactory As IEventTypeFactory
            Dim mapper As IMapper
            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                typeFactory = scope.Resolve(Of IEventTypeFactory)()
                innerType = typeFactory.Get(New Settings(), CType(id, enumEventType))

                If innerType Is Nothing Then
                    result = NotFound()
                End If

                If result Is Nothing Then
                    mapper = New Mapper(m_mapperConfiguration)
                    result = Ok(mapper.Map(Of EventType)(innerType))
                End If
            End Using
            Return result
        End Function

        'todo add error handling
        <HttpPut(), ClaimsAuthorization(ClaimTypes:="TA")> Public Function SaveEventType(<FromBody()> ByVal eventType As EventType) As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim innerEventType As IEventType = Nothing
            Dim typeSaver As IEventTypeSaver
            Dim typeFactory As IEventTypeFactory
            Dim mapper As IMapper
            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                typeFactory = scope.Resolve(Of IEventTypeFactory)()
                innerEventType = typeFactory.Get(New Settings(), CType(eventType.EventTypeId, enumEventType))

                If innerEventType Is Nothing Then
                    result = NotFound()
                End If

                If result Is Nothing Then
                    mapper = New Mapper(m_mapperConfiguration)
                    mapper.Map(Of EventType, IEventType)(eventType, innerEventType)
                    typeSaver = scope.Resolve(Of IEventTypeSaver)()
                    typeSaver.Update(New Settings(), innerEventType)
                    result = Ok(mapper.Map(Of EventType)(innerEventType))
                End If
            End Using
            Return result
        End Function
    End Class
End Namespace