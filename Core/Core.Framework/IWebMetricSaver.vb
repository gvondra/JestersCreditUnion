Public Interface IWebMetricSaver
    Sub Create(ByVal settings As ISettings,
             ByVal url As String,
             ByVal method As String,
             ByVal createTimestamp As Date,
             ByVal duration As Double,
             ByVal status As String,
             ByVal controller As String,
             Optional ByVal attributes As IDictionary(Of String, String) = Nothing)

End Interface
