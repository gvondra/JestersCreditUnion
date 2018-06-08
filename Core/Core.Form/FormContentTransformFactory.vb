Public Class FormContentTransformFactory
    Implements IFormContentTransormFactory

    Public Function GetTransform(form As IForm) As IFormContentTransform Implements IFormContentTransormFactory.GetTransform
        Return New FormContentTransform(form)
    End Function
End Class
