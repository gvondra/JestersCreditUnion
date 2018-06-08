Public Interface IFormSerializerFactory
    Function Create(Of T)(ByVal form As T) As IFormSerializer
End Interface
