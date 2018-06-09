Imports System.Net
Imports System.Security.Claims
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class MenuController
        Inherits ControllerBase

        <HttpGet(), Authorize()> Public Function GetMenuItems() As IHttpActionResult
            Dim column1 As New ArrayList
            Dim column2 As New ArrayList
            Dim column3 As New ArrayList
            Dim result As New ArrayList
            Dim flags As enumRole = RoleProcessor.GetRoleFlags(CType(Me.User, ClaimsPrincipal))

            column1.Add(New With {.Text = "Home", .URL = ""})

            If (flags And enumRole.TaskAdministrator) = enumRole.TaskAdministrator _
                    OrElse (flags And enumRole.TaskProcessor) = enumRole.TaskProcessor Then

                column1.Add(New With {.Text = "Unassigned Tasks", .URL = "unassignedtasks"})
                column1.Add(New With {.Text = "My Tasks", .URL = "mytasks"})
            End If

            If flags = enumRole.None Then
                column1.Add(New With {.Text = "Role Request", .URL = "rolerequest"})
            End If

            If (flags And enumRole.TaskAdministrator) = enumRole.TaskAdministrator Then
                column2.Add(New With {.Text = "Events", .URL = "eventtypelist"})
                column2.Add(New With {.Text = "Tasks", .URL = "tasktypelist"})
            End If

            If (flags And enumRole.TaskAdministrator) = enumRole.TaskAdministrator _
                    OrElse (flags And enumRole.UserAdministrator) = enumRole.UserAdministrator Then

                column2.Add(New With {.Text = "Groups", .URL = "grouplist"})
            End If

            If (flags And enumRole.UserAdministrator) = enumRole.UserAdministrator Then
                column2.Add(New With {.Text = "Users", .URL = "usersearch"})
            End If

            If flags <> enumRole.None Then
                column3.Add(New With {.Text = "Web Metrics", .URL = "webmetrics"})
                column3.Add(New With {.Text = "Exceptions", .URL = ""})
            End If

            If column1.Count > 0 Then
                result.Add(column1)
            End If
            If column2.Count > 0 Then
                result.Add(column2)
            End If
            If column3.Count > 0 Then
                result.Add(column3)
            End If

            Return Ok(result)
        End Function
    End Class
End Namespace