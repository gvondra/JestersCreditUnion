Imports AutoMapper
Public Class TaskMapper
    Public Property MapperConfiguration As MapperConfiguration
    Public Property Settings As Settings = New Settings()

    Public Sub New()
        MapperConfiguration = New MapperConfiguration(Sub(c As IMapperConfigurationExpression)
                                                          AddTaskTypeTitleMapper(
                                                          AddOwnerNameMapper(
                                                            c.CreateMap(Of ITask, Task)()
                                                          ))
                                                      End Sub)
    End Sub

    Public Function AddOwnerNameMapper(ByVal exp As IMappingExpression(Of ITask, Task)) As IMappingExpression(Of ITask, Task)
        exp.ForMember(Of String)(
            Function(t As Task) t.TaskOwnerName,
            Sub(opt As IMemberConfigurationExpression(Of ITask, Task, String)) opt.ResolveUsing(Of String)(AddressOf OwnerNameResolver)
        )
        Return exp
    End Function

    Public Function OwnerNameResolver(ByVal source As ITask, ByVal destination As Task) As String
        Dim owner As IUser = source.GetUser(Me.Settings)
        If owner IsNot Nothing Then
            Return owner.FullName
        Else
            Return Nothing
        End If
    End Function

    Public Function AddTaskTypeTitleMapper(ByVal exp As IMappingExpression(Of ITask, Task)) As IMappingExpression(Of ITask, Task)
        exp.ForMember(Of String)(
            Function(t As Task) t.TaskTypeTitle,
            Sub(opt As IMemberConfigurationExpression(Of ITask, Task, String)) opt.ResolveUsing(Of String)(AddressOf TaskTypeTitleResolver)
        )
        Return exp
    End Function

    Public Function TaskTypeTitleResolver(ByVal source As ITask, ByVal destination As Task) As String
        Dim taskType As ITaskType = source.GetTaskType(Me.Settings)
        Return taskType.Title
    End Function
End Class
