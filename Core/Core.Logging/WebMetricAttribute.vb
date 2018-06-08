Imports JestersCreditUnion.DataTier.Core.Models
Public Class WebMetricAttribute
    Implements IWebMetricAttribute

    Private m_webMetricAttributeData As WebMetricAttributeData

    Friend Sub New(ByVal attributeData As WebMetricAttributeData)
        m_webMetricAttributeData = attributeData
    End Sub

    Public ReadOnly Property Key As String Implements IWebMetricAttribute.Key
        Get
            Return m_webMetricAttributeData.Key
        End Get
    End Property

    Public ReadOnly Property Value As String Implements IWebMetricAttribute.Value
        Get
            Return m_webMetricAttributeData.Value
        End Get
    End Property

    Public ReadOnly Property WebMetricAttributeId As Int32 Implements IWebMetricAttribute.WebMetricAttributeId
        Get
            Return m_webMetricAttributeData.WebMetricAttributeId
        End Get
    End Property
End Class
