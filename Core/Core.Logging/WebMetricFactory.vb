Imports JestersCreditUnion.DataTier.Core
Imports Autofac
Public Class WebMetricFactory
    Implements IWebMetricFactory

    Private m_container As IContainer

    Public Sub New()
        m_container = ObjectContainer.GetContainer
    End Sub

    Public Function GetByMaxCreateTimestamp(settings As ISettings, until As DateTime, page As Int32) As IEnumerable(Of IWebMetric) Implements IWebMetricFactory.GetByMaxCreateTimestamp
        Const PAGE_SIZE As Integer = 20
        Dim factory As IWebMetricDataFactory
        Dim result As IEnumerable(Of IWebMetric)
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of IWebMetricDataFactory)()
            result = From d In factory.GetByMaxCreateTimestamp(New Settings(settings), until, page * PAGE_SIZE, PAGE_SIZE)
                     Select New WebMetric(d)
        End Using
        Return result
    End Function
End Class
