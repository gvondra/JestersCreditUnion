Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class WebMetricSaver
    Implements IWebMetricSaver

    Private m_container As IContainer

    Public Sub New()
        m_container = ObjectContainer.GetContainer
    End Sub

    Public Sub Create(settings As ISettings, url As String, method As String, createTimestamp As DateTime, duration As Double, status As String, controller As String, Optional attributes As IDictionary(Of String, String) = Nothing) Implements IWebMetricSaver.Create
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), Sub(th As ITransactionHandler) InnerCreate(th, url, method, createTimestamp, duration, status, controller, attributes))
    End Sub

    Private Sub InnerCreate(transactionHandler As ITransactionHandler, url As String, method As String, createTimestamp As DateTime, duration As Double, status As String, controller As String, attributes As IDictionary(Of String, String))
        Dim creator As DataTier.Utilities.IDataCreator
        Dim webMetric As WebMetricData
        Dim attribute As WebMetricAttributeData

        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            webMetric = New WebMetricData() With {
                .Url = url,
                .Method = method,
                .CreateTimestamp = createTimestamp,
                .Duration = duration,
                .Status = status,
                .Controller = controller
            }

            creator = scope.Resolve(Of WebMetricDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(WebMetricData), webMetric)
            )
            creator.Create()

            If attributes IsNot Nothing Then
                For Each kp As KeyValuePair(Of String, String) In attributes
                    attribute = New WebMetricAttributeData() With {
                        .WebMetricId = webMetric.WebMetricId,
                        .Key = kp.Key,
                        .Value = kp.Value
                    }
                    creator = scope.Resolve(Of WebMetricAttributeDataSaver)(
                        New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                        New TypedParameter(GetType(WebMetricAttributeData), attribute)
                    )
                    creator.Create()
                Next kp
            End If
        End Using
    End Sub
End Class
