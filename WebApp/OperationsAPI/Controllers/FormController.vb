Imports Autofac
Imports AutoMapper
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Security.Claims
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class FormController
        Inherits ControllerBase

        Private Shared m_mapperConfiguration As MapperConfiguration

        Shared Sub New()
            m_mapperConfiguration = New MapperConfiguration(Sub(c As IMapperConfigurationExpression)
                                                                c.CreateMap(Of RoleRequest, Forms.IRoleRequest)()
                                                            End Sub)
        End Sub

        <HttpGet(), ClaimsAuthorization(ClaimTypes:="ANY")> Public Function GetForm(ByVal id As Guid) As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim innerForm As IForm = Nothing
            Dim formFactory As IFormFactory
            Dim mapper As IMapper
            Dim mapperConfiguration As FormMapper

            If id.Equals(Guid.Empty) Then
                result = BadRequest("Missing or invalid form id")
            End If

            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                formFactory = scope.Resolve(Of IFormFactory)()
                If result Is Nothing Then
                    innerForm = formFactory.Get(New Settings(), id)
                    If innerForm Is Nothing Then
                        result = NotFound()
                    End If
                End If

                If result Is Nothing Then
                    mapperConfiguration = New FormMapper
                    mapper = New Mapper(mapperConfiguration.MapperConfiguration)
                    result = Ok(mapper.Map(Of Form)(innerForm))
                End If
            End Using

            Return result
        End Function

        <HttpGet(), ClaimsAuthorization(ClaimTypes:="ANY"), Route("api/Form/{id}/html")> Public Function GetFormHtml(ByVal id As Guid) As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim innerForm As IForm = Nothing
            Dim formFactory As IFormFactory
            Dim formTransformFactory As IFormContentTransormFactory
            Dim formTransform As IFormContentTransform

            If id.Equals(Guid.Empty) Then
                result = BadRequest("Missing or invalid form id")
            End If

            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                formFactory = scope.Resolve(Of IFormFactory)()
                If result Is Nothing Then
                    innerForm = formFactory.Get(New Settings(), id)
                    If innerForm Is Nothing Then
                        result = NotFound()
                    End If
                End If

                If result Is Nothing Then
                    formTransformFactory = scope.Resolve(Of IFormContentTransormFactory)()
                    formTransform = formTransformFactory.GetTransform(innerForm)

                    Using stream As Stream = formTransform.Transform()
                        Using reader As StreamReader = New StreamReader(stream)
                            Dim responseMessage As New HttpResponseMessage(HttpStatusCode.OK)
                            responseMessage.Content = New StringContent(reader.ReadToEnd, Encoding.UTF8, "text/html")
                            result = Me.ResponseMessage(responseMessage)
                        End Using
                    End Using
                End If
            End Using

            Return result
        End Function

    End Class
End Namespace