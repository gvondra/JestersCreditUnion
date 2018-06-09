Public Class WebMetric
    Public Property WebMetricId As Integer
    Public Property Url As String
    Public Property Method As String
    Public Property CreateTimestamp As Date
    Public Property Duration As Double
    Public Property Status As String
    Public Property Controller As String
    Public Property Attributes As List(Of WebMetricAttribute)
End Class
