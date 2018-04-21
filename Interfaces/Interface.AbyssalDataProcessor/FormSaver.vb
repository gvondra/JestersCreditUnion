Imports System.IO
Public Class FormSaver
    Implements IFormSaver

    Public Function CreateForm(settings As ISettings, accessToken As String, request As CreateFormRequest) As Form Implements IFormSaver.CreateForm
        Dim webRequest As New WebRequest()
        Dim uriBuilder As New UriBuilder(settings.BaseAddress)

        uriBuilder.Path &= "Forms"
        Return webRequest.Post(Of Form)(uriBuilder.Uri, accessToken,
            Sub(s As Stream) webRequest.Serialize(s, request)
        )
    End Function
End Class
