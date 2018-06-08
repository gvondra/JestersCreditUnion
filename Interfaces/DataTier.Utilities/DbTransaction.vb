Public Class DbTransaction
    Implements IDbTransaction

    Private m_disposedValue As Boolean ' To detect redundant calls
    Private m_innerTransaction As Data.IDbTransaction
    Private m_observers As New List(Of IDbTransactionObserver)

    Public Sub New(ByVal transaction As Data.IDbTransaction)
        m_innerTransaction = transaction
    End Sub

    Public ReadOnly Property InnerTransaction As System.Data.IDbTransaction Implements IDbTransaction.InnerTransaction
        Get
            Return m_innerTransaction
        End Get
    End Property

    Public ReadOnly Property Connection As IDbConnection Implements Data.IDbTransaction.Connection
        Get
            Return m_innerTransaction.Connection
        End Get
    End Property

    Public ReadOnly Property IsolationLevel As IsolationLevel Implements Data.IDbTransaction.IsolationLevel
        Get
            Return m_innerTransaction.IsolationLevel
        End Get
    End Property

    Public Sub AddObserver(observer As IDbTransactionObserver) Implements IDbTransaction.AddObserver
        If m_observers.Contains(observer) = False Then
            m_observers.Add(observer)
        End If
    End Sub

    Public Sub Commit() Implements Data.IDbTransaction.Commit
        m_innerTransaction.Commit()
        For Each observer As IDbTransactionObserver In m_observers
            observer.AfterCommit()
        Next
    End Sub

    Public Sub Rollback() Implements Data.IDbTransaction.Rollback
        m_innerTransaction.Rollback()
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not m_disposedValue Then
            If disposing Then
                If m_innerTransaction IsNot Nothing Then
                    m_innerTransaction.Dispose()
                End If
            End If

            ' free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' set large fields to null.
        End If
        m_disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

End Class
