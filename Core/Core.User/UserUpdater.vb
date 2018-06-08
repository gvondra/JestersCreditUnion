Imports JestersCreditUnion.Interface.UserManagement
Imports Autofac
Public Class UserUpdater
    Implements IUserUpdater

    Private m_container As IContainer

    Public Sub New()
        m_container = ObjectContainer.GetContainer()
    End Sub

    Public Sub New(ByVal container As IContainer)
        m_container = container
    End Sub

    Public Sub UpdateFromUserManagement(settings As Utilities.Framework.ISettings, user As IUser, ByVal accessToken As String) Implements IUserUpdater.UpdateFromUserManagement
        Dim service As IUserInfoService
        Dim userInfo As UserInfo

        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            service = scope.Resolve(Of IUserInfoService)(New Autofac.TypedParameter(GetType([Interface].UserManagement.ISettings), New UserManagementSettings(settings)))
            userInfo = service.Get(accessToken)
        End Using

        If userInfo IsNot Nothing Then
            If String.IsNullOrEmpty(userInfo.name) = False Then
                user.FullName = userInfo.name
            End If
            If String.IsNullOrEmpty(userInfo.given_name) = False Then
                user.ShortName = userInfo.given_name
            End If
            If String.IsNullOrEmpty(userInfo.email) = False Then
                user.EmailAddress = userInfo.email
            End If
        End If
    End Sub
End Class

