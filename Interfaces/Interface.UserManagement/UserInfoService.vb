Imports Newtonsoft.Json
Imports System.IO
Imports System.Net
Public Class UserInfoService
    Implements IUserInfoService

    Private m_settings As ISettings

    Public Sub New(ByVal settings As ISettings)
        m_settings = settings
    End Sub

    Public Function [Get](accessToken As String) As UserInfo Implements IUserInfoService.Get
        Dim webResponse As HttpWebResponse
        Dim webRequest As HttpWebRequest = CType(Net.WebRequest.Create($"https://{m_settings.EndpointDomain}/UserInfo"), HttpWebRequest)
        Dim reader As StreamReader
        Dim userInfo As UserInfo

        webRequest.Accept = "application/json"
        webRequest.UseDefaultCredentials = False
        webRequest.Headers.Add("Authorization", "Bearer " & accessToken)
        webRequest.Method = WebRequestMethods.Http.Get
        webResponse = CType(webRequest.GetResponse(), HttpWebResponse)
        Using stream As Stream = webResponse.GetResponseStream()
            reader = New StreamReader(stream)
            userInfo = JsonConvert.DeserializeObject(Of UserInfo)(reader.ReadToEnd)
        End Using
        Return userInfo
    End Function
End Class
