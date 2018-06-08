Imports System.IO
Imports System.Text
Imports System.Xml
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class FormContentTransformTest

    Private Const TEST_XML As String = "<RoleRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""abyssaldataprocessor/forms/rolerequest/v1"">
  <FullName>Test Fule Name</FullName>
  <Comment>Request details</Comment>
</RoleRequest>"

    <TestMethod()>
    Public Sub TransformTestTest()
        Dim content As New XmlDocument()
        Dim form As New Mock(Of IForm)
        Dim transform As New FormContentTransform(form.Object)

        content.LoadXml(TEST_XML)

        form.SetupGet(Of enumFormStyle)(Function(f As IForm) f.Style).Returns(enumFormStyle.RoleRequest)
        form.SetupGet(Of XmlNode)(Function(f As IForm) f.Content).Returns(content)

        Using stream As Stream = transform.Transform()
            Assert.IsNotNull(stream)
        End Using
    End Sub

End Class
