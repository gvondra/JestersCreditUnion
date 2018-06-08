Imports Autofac
Imports System.Security.Claims
Imports System.Web.Http
Public MustInherit Class ControllerBase
    Inherits ApiController

    Private m_container As IContainer

    Public Sub New()
        ' todo get containter
    End Sub

    Public Property ObjectContainer As IContainer
        Get
            Return m_container
        End Get
        Set(value As IContainer)
            m_container = value
        End Set
    End Property

    <NonAction()> Public Function GetUserObject() As IUser
        Dim factory As IUserFactory
        Dim user As IUser
        Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
            factory = scope.Resolve(Of IUserFactory)()
            user = factory.Get(CType(Me.User, ClaimsPrincipal))
        End Using
        Return user
    End Function
End Class
