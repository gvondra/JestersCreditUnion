Public Class FormSerializerFactory
    Implements IFormSerializerFactory

    Public Function Create(Of T)(form As T) As IFormSerializer Implements IFormSerializerFactory.Create
        Return New FormSerializer(Of T)(form)
    End Function
End Class
