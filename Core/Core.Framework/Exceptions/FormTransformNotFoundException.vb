Public Class FormTransformNotFoundException
    Inherits ApplicationException

    Public Sub New(ByVal message As String, ByVal style As enumFormStyle)
        MyBase.New(message)
        Me.Data.Add("Form Style", style.ToString)
    End Sub

    Public Sub New(ByVal message As String, ByVal style As enumFormStyle, ByVal resourceName As String)
        MyBase.New(message)
        Me.Data.Add("Form Style", style.ToString)
        Me.Data.Add("Resource Name", resourceName)
    End Sub
End Class
