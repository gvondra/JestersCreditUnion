Imports JestersCreditUnion.Core.Utilities
Imports AutoMapper
Public NotInheritable Class UserMapper
    Public Property MapperConfiguration As MapperConfiguration

    Public Sub New()
        MapperConfiguration = New MapperConfiguration(Sub(c As IMapperConfigurationExpression)
                                                          AddPhoneNumberFormatter(c.CreateMap(Of IUser, User)())
                                                          c.CreateMap(Of User, IUser)()
                                                      End Sub)
    End Sub

    Public Function AddPhoneNumberFormatter(ByVal exp As IMappingExpression(Of IUser, User)) As IMappingExpression(Of IUser, User)
        exp.ForMember(Of String)(Function(u As User) u.PhoneNumber,
            Sub(opt As IMemberConfigurationExpression(Of IUser, User, String)) opt.AddTransform(Function(v As String) StringFormatters.FormatPhoneNumber(v))
        )
        Return exp
    End Function
End Class
