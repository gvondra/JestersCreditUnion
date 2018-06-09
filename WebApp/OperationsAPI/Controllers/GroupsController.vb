Imports Autofac
Imports AutoMapper
Imports System.Net
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class GroupsController
        Inherits ControllerBase

        Private Shared m_mapperConfiguration As MapperConfiguration

        Shared Sub New()
            m_mapperConfiguration = New MapperConfiguration(Sub(exp As IMapperConfigurationExpression)
                                                                exp.CreateMap(Of IGroup, Group)()
                                                            End Sub)
        End Sub

        'todo add error handling
        <HttpGet(), ClaimsAuthorization(ClaimTypes:="UA|TA")> Public Shadows Function GetAll() As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim groups As List(Of Group)
            Dim groupFactory As IGroupFactory
            Dim mapper As IMapper
            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                groupFactory = scope.Resolve(Of IGroupFactory)()
                mapper = New Mapper(m_mapperConfiguration)
                groups = New List(Of Group)(
                    From innerGroup In groupFactory.GetAll(New Settings())
                    Select mapper.Map(Of Group)(innerGroup)
                )

                result = Ok(groups)
            End Using
            Return result
        End Function
    End Class
End Namespace