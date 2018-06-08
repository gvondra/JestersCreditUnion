Public Interface IWebMetric
    ReadOnly Property WebMetricId As Integer
    ReadOnly Property Url As String
    ReadOnly Property Method As String
    ReadOnly Property CreateTimestamp As Date
    ReadOnly Property Duration As Double
    ReadOnly Property Status As String
    ReadOnly Property Controller As String

    Function GetAttributes() As IEnumerable(Of IWebMetricAttribute)
End Interface
