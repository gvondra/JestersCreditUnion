Imports System.Xml
Namespace Forms
    Public Interface IFormSerializable
        Function Serialize() As XmlNode
        Function CreateForm(ByVal user As IUser) As IForm
    End Interface
End Namespace