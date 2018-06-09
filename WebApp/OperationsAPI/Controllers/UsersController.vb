Imports Autofac
Imports AutoMapper
Imports System.Text.RegularExpressions
Imports System.Net
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class UsersController
        Inherits ControllerBase

        Private Shared UserMapper As UserMapper

        Shared Sub New()
            UserMapper = New UserMapper
        End Sub

        <HttpGet(), ClaimsAuthorization(ClaimTypes:="UA"), Route("api/Users/Search")>
        Function Search(ByVal s As String) As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim userId As Guid?
            Dim userFactory As JestersCreditUnion.CommonAPI.IUserFactory
            Dim innerUsers As IEnumerable(Of IUser) = Nothing
            Dim u As IUser
            Dim users As IEnumerable(Of User)
            Dim mapper As IMapper

            If String.IsNullOrEmpty(s) Then
                result = BadRequest("Missing search text")
            End If

            If result Is Nothing Then
                Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                    userFactory = scope.Resolve(Of JestersCreditUnion.CommonAPI.IUserFactory)()
                    userId = SearchToGuid(s)
                    If userId.HasValue Then
                        u = userFactory.Get(New Settings(), userId.Value)
                        If u IsNot Nothing Then
                            innerUsers = {u}
                        Else
                            innerUsers = New List(Of IUser)()
                        End If
                    Else
                        innerUsers = userFactory.Search(New Settings(), s)
                    End If
                End Using
            End If

            If result Is Nothing Then
                mapper = New Mapper(UserMapper.MapperConfiguration)
                users = From y In innerUsers.Select(Of User)(Function(x As IUser) mapper.Map(Of User)(x))
                result = Ok(users)
            End If

            Return result
        End Function

        Private Function SearchToGuid(ByVal s As String) As Guid?
            Dim g As Guid? = Nothing
            Dim h As Guid
            s = Regex.Replace(s, "[^0-9A-Fa-f]+", String.Empty)
            If Regex.IsMatch(s, "[0-9A-Fa-f]{32}") Then
                If Guid.TryParse(s, h) Then
                    g = h
                End If
            End If
            Return g
        End Function
    End Class
End Namespace