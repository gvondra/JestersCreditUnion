Public Interface IWebMetricDataFactory
    Function GetByMaxCreateTimestamp(ByVal settings As ISettings, ByVal until As Date, ByVal offset As Integer, ByVal rowLimit As Integer) As IEnumerable(Of WebMetricData)
End Interface
