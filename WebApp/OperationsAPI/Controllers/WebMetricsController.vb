Imports Autofac
Imports AutoMapper
Imports System.Net
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class WebMetricsController
        Inherits ControllerBase

        Private Shared m_mapperConfiguration As MapperConfiguration

        Shared Sub New()
            m_mapperConfiguration = New MapperConfiguration(
                Sub(exp As IMapperConfigurationExpression)
                    exp.CreateMap(Of IWebMetric, WebMetric)()
                    exp.CreateMap(Of IWebMetricAttribute, WebMetricAttribute)()
                End Sub
            )
        End Sub

        <HttpGet(), Authorize()> Public Function GetWebMetrics(ByVal until As Date, ByVal page As Integer) As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim factory As IWebMetricFactory
            Dim innerWebMetrics As IEnumerable(Of IWebMetric)
            Dim mapper As IMapper
            Dim webMetrics As IEnumerable(Of WebMetric)

            If until.Kind = DateTimeKind.Local Then
                until = until.ToUniversalTime
            End If

            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                factory = scope.Resolve(Of IWebMetricFactory)()
                innerWebMetrics = factory.GetByMaxCreateTimestamp(New Settings(), until, page)
                mapper = New Mapper(m_mapperConfiguration)
                webMetrics = From w In innerWebMetrics
                             Select MapWebMetric(mapper, w)
                result = Ok(webMetrics)
            End Using

            Return result
        End Function

        <NonAction> Private Function MapWebMetric(ByVal mapper As IMapper, ByVal webMetric As IWebMetric) As WebMetric
            Dim result As WebMetric = mapper.Map(Of WebMetric)(webMetric)
            Dim innerAttributes As IEnumerable(Of IWebMetricAttribute) = webMetric.GetAttributes
            If innerAttributes IsNot Nothing Then
                result.Attributes = New List(Of WebMetricAttribute)(
                    From a In innerAttributes
                    Select mapper.Map(Of WebMetricAttribute)(a)
                )
            End If
            Return result
        End Function
    End Class
End Namespace