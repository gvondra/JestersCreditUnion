Imports System.Text.RegularExpressions
Public NotInheritable Class StringFormatters
    Public Shared Function FormatPhoneNumber(ByVal value As String) As String
        Dim countryCode As String
        If String.IsNullOrEmpty(value) = False Then
            value = UnformatPhoneNumber(value)
            If Regex.IsMatch(value, "[0-9]{10,11}") Then
                If value.Length > 10 Then
                    countryCode = value.Substring(0, value.Length - 10)
                    value = value.Substring(value.Length - 10)
                Else
                    countryCode = String.Empty
                End If
                value = $"{value.Substring(0, 3)}-{value.Substring(3, 3)}-{value.Substring(6)}"
                If String.IsNullOrEmpty(countryCode) = False Then
                    value = countryCode & " " & value
                End If
            End If
        End If
        Return value
    End Function

    Public Shared Function UnformatPhoneNumber(ByVal value As String) As String
        If String.IsNullOrEmpty(value) = False Then
            value = Regex.Replace(value.Trim, "[^0-9]+", String.Empty)
        End If
        Return value
    End Function
End Class
