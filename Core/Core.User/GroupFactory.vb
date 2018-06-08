Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class GroupFactory
    Implements IGroupFactory

    Private m_container As IContainer

    Public Sub New()
        m_container = ObjectContainer.GetContainer()
    End Sub

    Public Function Create() As IGroup Implements IGroupFactory.Create
        Return New Group(New GroupData())
    End Function

    Public Function [Get](settings As ISettings, groupId As Guid) As IGroup Implements IGroupFactory.Get
        Dim data As GroupData
        Dim result As Group = Nothing
        Dim factory As IGroupDataFactory

        Using scope As ILifetimeScope = m_container.BeginLifetimeScope()
            factory = scope.Resolve(Of IGroupDataFactory)()
            data = factory.Get(New Settings(settings), groupId)
        End Using

        If data IsNot Nothing Then
            result = New Group(data)
        End If

        Return result
    End Function

    Public Function GetAll(settings As ISettings) As IEnumerable(Of IGroup) Implements IGroupFactory.GetAll
        Dim result As IEnumerable(Of Group) = {}
        Dim factory As IGroupDataFactory

        Using scope As ILifetimeScope = m_container.BeginLifetimeScope()
            factory = scope.Resolve(Of IGroupDataFactory)()
            result = From data In factory.GetAll(New Settings(settings))
                     Select New Group(data)
        End Using

        Return result
    End Function
End Class
