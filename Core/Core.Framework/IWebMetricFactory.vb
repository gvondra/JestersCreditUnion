Public Interface IWebMetricFactory
    Function GetByMaxCreateTimestamp(ByVal settings As ISettings, ByVal until As Date, ByVal page As Integer) As IEnumerable(Of IWebMetric)
End Interface
