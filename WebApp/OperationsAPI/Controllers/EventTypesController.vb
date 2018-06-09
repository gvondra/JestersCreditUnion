Imports Autofac
Imports AutoMapper
Imports System.Net
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class EventTypesController
        Inherits ControllerBase

        Private Shared m_mapperConfiguration As MapperConfiguration

        Shared Sub New()
            m_mapperConfiguration = New MapperConfiguration(Sub(exp As IMapperConfigurationExpression)
                                                                exp.CreateMap(Of IEventType, EventType)()
                                                            End Sub)
        End Sub

        'todo add error handling
        <HttpGet(), ClaimsAuthorization(ClaimTypes:="TA")> Public Shadows Function GetAll() As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim eventTypes As List(Of EventType)
            Dim eventTypeFactory As IEventTypeFactory
            Dim mapper As IMapper
            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                eventTypeFactory = scope.Resolve(Of IEventTypeFactory)()
                mapper = New Mapper(m_mapperConfiguration)
                eventTypes = New List(Of EventType)(
                    From innerEventType In eventTypeFactory.GetAll(New Settings())
                    Select mapper.Map(Of EventType)(innerEventType)
                )

                result = Ok(eventTypes)
            End Using
            Return result
        End Function
    End Class
End Namespace