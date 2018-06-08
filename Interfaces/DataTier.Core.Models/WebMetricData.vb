Public Class WebMetricData
    <ColumnMapping("WebMetricId")> Public Property WebMetricId As Integer
    <ColumnMapping("Url")> Public Property Url As String
    <ColumnMapping("Method")> Public Property Method As String
    <ColumnMapping("CreateTimestamp")> Public Property CreateTimestamp As Date
    <ColumnMapping("Duration")> Public Property Duration As Double
    <ColumnMapping("Status")> Public Property Status As String
    <ColumnMapping("Controller")> Public Property Controller As String

    Public Property Attributes As IEnumerable(Of WebMetricAttributeData)
End Class
