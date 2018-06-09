Imports AutoMapper
Public Class FormMapper
    Public Property MapperConfiguration As MapperConfiguration
    Public Property Settings As Settings = New Settings()

    Public Sub New()
        MapperConfiguration = New MapperConfiguration(Sub(c As IMapperConfigurationExpression)
                                                          AddOwnerNameMapper(
                                                            c.CreateMap(Of IForm, Form)()
                                                          )
                                                      End Sub)
    End Sub

    Public Function AddOwnerNameMapper(ByVal exp As IMappingExpression(Of IForm, Form)) As IMappingExpression(Of IForm, Form)
        exp.ForMember(Of String)(
            Function(f As Form) f.TaskOwnerName,
            Sub(opt As IMemberConfigurationExpression(Of IForm, Form, String)) opt.ResolveUsing(Of String)(AddressOf OwnerNameResolver)
        )
        Return exp
    End Function

    Public Function OwnerNameResolver(ByVal source As IForm, ByVal destination As Form) As String
        Dim owner As IUser = source.GetUser(Me.Settings)
        If owner IsNot Nothing Then
            Return owner.FullName
        Else
            Return Nothing
        End If
    End Function

End Class
