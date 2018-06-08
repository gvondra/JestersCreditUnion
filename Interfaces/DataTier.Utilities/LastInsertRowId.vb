Public Class LastInsertRowId
    Public Function GetLastInsertRowId(ByVal connection As IDbConnection) As Integer
        Dim id As Integer
        Using command As IDbCommand = connection.CreateCommand
            command.CommandType = CommandType.Text
            command.CommandText = "SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];"
            Return CType(command.ExecuteScalar, Integer)
        End Using
        Return id
    End Function
End Class
