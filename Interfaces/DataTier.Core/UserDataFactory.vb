Imports System.Text.RegularExpressions
Public Class UserDataFactory
    Implements IUserDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of UserData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of UserData)()
    End Sub

    Public Function GetByEmailAddress(settings As ISettings, emailAddress As String) As IEnumerable(Of UserData) Implements IUserDataFactory.GetByEmailAddress
        Return GetByEmailAddress(settings, New DbProviderFactory(), emailAddress)
    End Function

    Public Function GetByEmailAddress(settings As ISettings, ByVal providerFactory As IDbProviderFactory, emailAddress As String) As IEnumerable(Of UserData)
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "emailAddress", DbType.String)
        parameter.Value = emailAddress
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sUserByEmailAddress",
                                             Function() New UserData,
                                             New Action(Of IEnumerable(Of UserData))(AddressOf AssignDataStateManager(Of UserData)),
                                             {parameter})
    End Function

    Public Function GetBySubscriberId(settings As ISettings, subscriberId As String) As UserData Implements IUserDataFactory.GetBySubscriberId
        Return GetBySubscriberId(settings, New DbProviderFactory(), subscriberId)
    End Function

    Public Function GetBySubscriberId(settings As ISettings, ByVal providerFactory As IDbProviderFactory, subscriberId As String) As UserData
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "subscriberId", DbType.String)
        parameter.Value = subscriberId
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sUserBySubscriberId",
                                             Function() New UserData,
                                             New Action(Of IEnumerable(Of UserData))(AddressOf AssignDataStateManager(Of UserData)),
                                             {parameter}).FirstOrDefault
    End Function

    Public Function [Get](settings As ISettings, userId As Guid) As UserData Implements IUserDataFactory.Get
        Return Me.Get(settings, New DbProviderFactory, userId)
    End Function

    Public Function [Get](settings As ISettings, ByVal providerFactory As IDbProviderFactory, userId As Guid) As UserData
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "userId", DbType.Guid)
        parameter.Value = userId
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sUser",
                                             Function() New UserData,
                                             New Action(Of IEnumerable(Of UserData))(AddressOf AssignDataStateManager(Of UserData)),
                                             {parameter}).FirstOrDefault
    End Function

    Public Function Search(settings As ISettings, searchText As String) As IEnumerable(Of UserData) Implements IUserDataFactory.Search
        Return Search(settings, New DbProviderFactory(), searchText)
    End Function

    Public Function Search(settings As ISettings, ByVal providerFactory As IDbProviderFactory, searchText As String) As IEnumerable(Of UserData)
        searchText = searchText.Trim
        Dim value As IDbDataParameter = CreateParameter(providerFactory, "value", DbType.String)
        value.Value = searchText
        Dim wildCardValue As IDbDataParameter = CreateParameter(providerFactory, "wildCardValue", DbType.String)
        searchText = Regex.Replace(searchText, "\\", "\\")
        searchText = Regex.Replace(searchText, "_", "\_")
        searchText = Regex.Replace(searchText, "%", "\%")
        searchText = Regex.Replace(searchText, "\s+", "%")
        wildCardValue.Value = "%" & searchText & "%"
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sUserSearch",
                                             Function() New UserData,
                                             New Action(Of IEnumerable(Of UserData))(AddressOf AssignDataStateManager(Of UserData)),
                                             {value, wildCardValue})
    End Function
End Class
