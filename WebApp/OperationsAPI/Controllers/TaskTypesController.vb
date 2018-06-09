Imports Autofac
Imports AutoMapper
Imports System.Net
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class TaskTypesController
        Inherits ControllerBase

        Private Shared m_mapperConfiguration As MapperConfiguration

        Shared Sub New()
            m_mapperConfiguration = New MapperConfiguration(Sub(exp As IMapperConfigurationExpression)
                                                                exp.CreateMap(Of ITaskType, TaskType)()
                                                            End Sub)
        End Sub

        'todo add error handling
        <HttpGet(), ClaimsAuthorization(ClaimTypes:="TA")> Public Shadows Function GetAll() As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim taskTypes As List(Of TaskType)
            Dim taskTypeFactory As ITaskTypeFactory
            Dim mapper As IMapper
            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                taskTypeFactory = scope.Resolve(Of ITaskTypeFactory)()
                mapper = New Mapper(m_mapperConfiguration)
                taskTypes = New List(Of TaskType)(
                    From innerType In taskTypeFactory.GetAll(New Settings())
                    Select mapper.Map(Of TaskType)(innerType)
                )

                result = Ok(taskTypes)
            End Using
            Return result
        End Function

    End Class
End Namespace