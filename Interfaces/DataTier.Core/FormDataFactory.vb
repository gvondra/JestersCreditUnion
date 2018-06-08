Public Class FormDataFactory
    Implements IFormDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of FormData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of FormData)()
    End Sub

    Public Function [Get](settings As ISettings, formId As Guid) As FormData Implements IFormDataFactory.Get
        Return Me.Get(settings, New DbProviderFactory, formId)
    End Function

    Public Function [Get](settings As ISettings, ByVal providerFactory As IDbProviderFactory, formId As Guid) As FormData
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "formId", DbType.Guid)
        parameter.Value = formId
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sForm",
                                             Function() New FormData,
                                             New Action(Of IEnumerable(Of FormData))(AddressOf AssignDataStateManager(Of FormData)),
                                             {parameter}).FirstOrDefault
    End Function
End Class
