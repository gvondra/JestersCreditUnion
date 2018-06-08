Imports JestersCreditUnion.DataTier.Core.Models
Public Class WebMetric
    Implements IWebMetric

    Private m_webMetricData As WebMetricData
    Private m_attributes As IList(Of IWebMetricAttribute)

    Friend Sub New(ByVal webMetricData As WebMetricData)
        m_webMetricData = webMetricData
        If webMetricData.Attributes IsNot Nothing Then
            m_attributes = New List(Of IWebMetricAttribute)(
                From a In webMetricData.Attributes
                Select New WebMetricAttribute(a)
            )
        End If
    End Sub

    Public ReadOnly Property Controller As String Implements IWebMetric.Controller
        Get
            Return m_webMetricData.Controller
        End Get
    End Property

    Public ReadOnly Property CreateTimestamp As DateTime Implements IWebMetric.CreateTimestamp
        Get
            Return m_webMetricData.CreateTimestamp
        End Get
    End Property

    Public ReadOnly Property Duration As Double Implements IWebMetric.Duration
        Get
            Return m_webMetricData.Duration
        End Get
    End Property

    Public ReadOnly Property Method As String Implements IWebMetric.Method
        Get
            Return m_webMetricData.Method
        End Get
    End Property

    Public ReadOnly Property Status As String Implements IWebMetric.Status
        Get
            Return m_webMetricData.Status
        End Get
    End Property

    Public ReadOnly Property Url As String Implements IWebMetric.Url
        Get
            Return m_webMetricData.Url
        End Get
    End Property

    Public ReadOnly Property WebMetricId As Int32 Implements IWebMetric.WebMetricId
        Get
            Return m_webMetricData.WebMetricId
        End Get
    End Property


    Public Function GetAttributes() As IEnumerable(Of IWebMetricAttribute) Implements IWebMetric.GetAttributes
        Return m_attributes
    End Function

End Class
