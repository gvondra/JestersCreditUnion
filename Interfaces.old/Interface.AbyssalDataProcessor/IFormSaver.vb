Public Interface IFormSaver
    Function CreateForm(ByVal settings As ISettings, ByVal accessToken As String, ByVal request As CreateFormRequest) As Form
End Interface
