Imports JestersCreditUnion.Core.Utilities.Framework
Imports Autofac
Imports System.Security.Claims
Imports System.Web
Public Class UserFactory
    Implements IUserFactory

    Private m_innerUserFactory As JestersCreditUnion.Core.Framework.IUserFactory
    Private m_container As IContainer

    Public Sub New()
        Dim objectContainerFactory As New ObjectContainerFactory
        m_container = objectContainerFactory.Create
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            m_innerUserFactory = scope.Resolve(Of JestersCreditUnion.Core.Framework.IUserFactory)
        End Using
    End Sub

    Public Sub New(ByVal container As IContainer)
        m_container = container
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            m_innerUserFactory = scope.Resolve(Of JestersCreditUnion.Core.Framework.IUserFactory)
        End Using
    End Sub

    Public Function [Get](principal As ClaimsPrincipal) As IUser Implements IUserFactory.Get
        Dim id As Claim = principal.Claims.FirstOrDefault(Function(c As Claim) c.Type = ClaimTypes.NameIdentifier)
        Dim user As IUser = Nothing
        Dim claim As Claim = Nothing

        If id IsNot Nothing Then
            user = GetBySubscriberId(New Settings(), id.Value)
        End If

        If user Is Nothing Then
            claim = principal.Claims.FirstOrDefault(Function(c As Claim) c.Type = ClaimTypes.Email)
            If claim IsNot Nothing Then
                user = GetByEmailAddress(New Settings(), claim.Value)
                If user IsNot Nothing Then
                    UpdateUser(principal, user, id)
                End If
            End If
        End If

        If user Is Nothing Then
            user = Create()
            UpdateUser(principal, user, id)
        End If

        Return user
    End Function

    Private Sub UpdateUser(ByVal principal As ClaimsPrincipal, ByVal user As IUser, ByVal id As Claim)
        Dim claim As Claim = Nothing
        Dim accessToken As String

        Using scope = m_container.BeginLifetimeScope
            claim = principal.Claims.FirstOrDefault(Function(c As Claim) c.Type = ClaimTypes.Email)
            If claim IsNot Nothing AndAlso String.IsNullOrEmpty(claim.Value) = False AndAlso String.IsNullOrEmpty(user.EmailAddress) Then
                user.EmailAddress = claim.Value
            End If

            claim = principal.Claims.FirstOrDefault(Function(c As Claim) c.Type = ClaimTypes.Name)
            If claim IsNot Nothing AndAlso String.IsNullOrEmpty(claim.Value) = False Then
                If String.IsNullOrEmpty(user.FullName) Then
                    user.FullName = claim.Value
                End If
                If String.IsNullOrEmpty(user.ShortName) Then
                    user.ShortName = claim.Value
                End If
            End If

            claim = principal.Claims.FirstOrDefault(Function(c As Claim) c.Type = ClaimTypes.DateOfBirth)
            If claim IsNot Nothing AndAlso String.IsNullOrEmpty(claim.Value) = False AndAlso user.BirthDate.HasValue = False Then
                user.BirthDate = Date.Parse(claim.Value).Date
            End If

            If principal.Claims.Where(Function(c As Claim) c.Type = "gty" AndAlso c.Value = "client-credentials").Any() Then
                claim = principal.Claims.FirstOrDefault(Function(c As Claim) c.Type = ClaimTypes.NameIdentifier)
                If claim IsNot Nothing AndAlso String.IsNullOrEmpty(user.FullName) Then
                    user.FullName = claim.Value
                End If
            Else
                accessToken = CType(CType(principal.Identity, ClaimsIdentity).BootstrapContext, System.IdentityModel.Tokens.BootstrapContext).Token
                Dim updater As IUserUpdater = scope.Resolve(Of IUserUpdater)()
                updater.UpdateFromUserManagement(New Settings, user, accessToken)
            End If

            Dim saver As IUserSaver = scope.Resolve(Of IUserSaver)
            saver.Save(New Settings(), user, id.Value)
        End Using
    End Sub

    Public Function GetBySubscriberId(settings As ISettings, subscriberId As String) As IUser Implements JestersCreditUnion.Core.Framework.IUserFactory.GetBySubscriberId
        Return m_innerUserFactory.GetBySubscriberId(settings, subscriberId)
    End Function

    Public Function Create() As IUser Implements JestersCreditUnion.Core.Framework.IUserFactory.Create
        Return m_innerUserFactory.Create()
    End Function

    Public Function GetByEmailAddress(settings As ISettings, emailAddress As String) As IUser Implements JestersCreditUnion.Core.Framework.IUserFactory.GetByEmailAddress
        Return m_innerUserFactory.GetByEmailAddress(settings, emailAddress)
    End Function

    Public Function [Get](settings As ISettings, userId As Guid) As IUser Implements JestersCreditUnion.Core.Framework.IUserFactory.Get
        Return m_innerUserFactory.Get(settings, userId)
    End Function

    Public Function Search(settings As ISettings, searchText As String) As IEnumerable(Of IUser) Implements JestersCreditUnion.Core.Framework.IUserFactory.Search
        Return m_innerUserFactory.Search(settings, searchText)
    End Function
End Class
