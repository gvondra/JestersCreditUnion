Imports Autofac
Imports System.Threading
Public Class ObjectContainerFactory
    Private Shared m_container As IContainer
    Private Shared m_lock As New AutoResetEvent(True)

    Public Function Create() As IContainer
        If m_container Is Nothing Then
            m_lock.WaitOne()
            Try
                If m_container Is Nothing Then
                    m_container = Build()
                End If
            Finally
                m_lock.Set()
            End Try
        End If
        Return m_container
    End Function

    Private Function Build() As IContainer
        Dim containerBuilder As New ContainerBuilder
        Dim builder As IObjectContainerBuilder

        builder = New Core.Event.ObjectContainerBuilder
        builder.Register(containerBuilder)

        builder = New Core.Form.ObjectContainerBuilder
        builder.Register(containerBuilder)

        builder = New Core.Log.ObjectContainerBuilder
        builder.Register(containerBuilder)

        builder = New Core.Task.ObjectContainerBuilder
        builder.Register(containerBuilder)

        builder = New Core.User.ObjectContainerBuilder
        builder.Register(containerBuilder)

        containerBuilder.RegisterType(Of UserFactory)().As(Of IUserFactory)()

        Return containerBuilder.Build
    End Function
End Class
