Imports Autofac
Imports System.Security.Claims
Imports System.Web.Http
Public Class ControllerBase
    Inherits ApiController

    Private m_container As IContainer

    Public Property ObjectContainer As IContainer
        Get
            If m_container Is Nothing Then
                m_container = JestersCreditUnionAPIForms.ObjectContainer.GetContainer()
            End If
            Return m_container
        End Get
        Set(value As IContainer)
            m_container = value
        End Set
    End Property

End Class
