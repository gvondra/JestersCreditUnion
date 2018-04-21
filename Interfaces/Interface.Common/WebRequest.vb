Imports Newtonsoft.Json
Imports System.IO
Imports System.Net
Public Class WebRequest
    Public Function [Get](Of T)(ByVal url As Uri,
                                ByVal accessToken As String) As T
        Dim webResponse As HttpWebResponse
        Dim webRequest As HttpWebRequest _
            = CreateWebRequest(
                url,
                accessToken,
                WebRequestMethods.Http.Get
            )
        Dim reader As StreamReader
        Dim result As T

        webResponse = CType(webRequest.GetResponse(), HttpWebResponse)
        Using stream As Stream = webResponse.GetResponseStream()
            reader = New StreamReader(stream)
            result = JsonConvert.DeserializeObject(Of T)(reader.ReadToEnd)
        End Using
        Return result
    End Function

    Public Function Post(Of T)(ByVal url As Uri,
                                ByVal accessToken As String,
                                ByVal serializeBody As Action(Of Stream)) As T

        Dim webResponse As HttpWebResponse
        Dim webRequest As HttpWebRequest _
            = CreateWebRequest(
                url,
                accessToken,
                WebRequestMethods.Http.Post
            )
        Dim reader As StreamReader
        Dim result As T

        serializeBody.Invoke(webRequest.GetRequestStream)

        webResponse = CType(webRequest.GetResponse(), HttpWebResponse)
        Using stream As Stream = webResponse.GetResponseStream()
            reader = New StreamReader(stream)
            result = JsonConvert.DeserializeObject(Of T)(reader.ReadToEnd)
        End Using
        Return result
    End Function

    Public Sub Serialize(ByVal stream As Stream, ByVal value As Object)
        Dim serializer As JsonSerializer
        Using writer As New StreamWriter(stream)
            serializer = New JsonSerializer()
            serializer.Serialize(writer, value)
            writer.Flush()
        End Using
    End Sub

    Public Function CreateWebRequest(ByVal url As Uri,
                                ByVal accessToken As String,
                                ByVal method As String) As HttpWebRequest
        Dim webRequest As HttpWebRequest = CType(Net.WebRequest.Create(url), HttpWebRequest)

        webRequest.Accept = "application/json"
        If String.Compare(method, WebRequestMethods.Http.Get, True) <> 0 Then
            webRequest.ContentType = "application/json"
        End If
        webRequest.UseDefaultCredentials = False
        If String.IsNullOrEmpty(accessToken) = False Then
            webRequest.Headers.Add("Authorization", "Bearer " & accessToken)
        End If
        webRequest.Method = method

        Return webRequest
    End Function
End Class
