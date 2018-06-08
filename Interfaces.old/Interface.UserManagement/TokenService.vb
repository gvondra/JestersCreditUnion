Imports Newtonsoft.Json
Imports System.IO
Imports System.Net
Public Class TokenService
    Implements ITokenService

    Private m_settings As ISettings

    Public Sub New(ByVal settings As ISettings)
        m_settings = settings
    End Sub

    Public Function GetToken(clientId As String, secret As String, audience As String) As Token Implements ITokenService.GetToken
        Dim webResponse As HttpWebResponse
        Dim webRequest As HttpWebRequest = CType(Net.WebRequest.Create($"https://{m_settings.EndpointDomain}/oauth/token"), HttpWebRequest)
        Dim reader As StreamReader
        Dim token As Token
        Dim serializer As JsonSerializer

        webRequest.Accept = "application/json"
        webRequest.ContentType = "application/json"
        webRequest.UseDefaultCredentials = False
        webRequest.Method = WebRequestMethods.Http.Post
        Using writer As New StreamWriter(webRequest.GetRequestStream)
            serializer = New JsonSerializer()
            serializer.Serialize(writer, New With {.client_id = clientId, .client_secret = secret, .audience = audience, .grant_type = "client_credentials"})
            writer.Flush()
        End Using
        webResponse = CType(webRequest.GetResponse(), HttpWebResponse)
        Using stream As Stream = webResponse.GetResponseStream()
            reader = New StreamReader(stream)
            token = JsonConvert.DeserializeObject(Of Token)(reader.ReadToEnd)
        End Using
        Return token
    End Function
End Class
