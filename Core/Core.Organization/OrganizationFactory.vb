Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class OrganizationFactory
    Implements IOrganizationFactory

    Private m_container As IContainer

    Public Sub New()
        m_container = ObjectContainer.GetContainer
    End Sub

    Public Function Create() As IOrganization Implements IOrganizationFactory.Create
        Return New Organization()
    End Function

    Public Function [Get](settings As ISettings, id As Guid) As IOrganization Implements IOrganizationFactory.Get
        Dim factory As IOrganizationDataFactory
        Dim data As OrganizationData
        Dim result As Organization = Nothing
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of IOrganizationDataFactory)
            data = factory.Get(New Settings(settings), id)
            If data IsNot Nothing Then
                result = New Organization(data)
            End If
        End Using
        Return result
    End Function

    Public Function Search(settings As ISettings, searchText As String) As IEnumerable(Of IOrganization) Implements IOrganizationFactory.Search
        Dim factory As IOrganizationDataFactory
        Dim result As IEnumerable(Of IOrganization)
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of IOrganizationDataFactory)
            result = From o In factory.Search(New Settings(settings), searchText)
                     Select New Organization(o)
        End Using
        Return result
    End Function
End Class
