Imports System.Reflection
Public Class DataStateManager(Of T)
    Implements IDataStateManager(Of T)

    Public Property Original As T Implements IDataStateManager(Of T).Original

    Public Sub New(ByVal original As T)
        Me.Original = original
    End Sub

    Public Sub New()
    End Sub

    Public Function GetState(ByVal target As T) As IDataStateManager(Of T).enumState Implements IDataStateManager(Of T).GetState
        Dim state As IDataStateManager(Of T).enumState = IDataStateManager(Of T).enumState.Unchaged

        If Me.Original Is Nothing Then
            state = IDataStateManager(Of T).enumState.New
        ElseIf target IsNot Nothing AndAlso CType(target, Object) IsNot CType(Me.Original, Object) Then
            If GetProperties(GetType(T)).Any(Function(p As PropertyInfo) IsChanged(p, target)) Then
                state = IDataStateManager(Of T).enumState.Updated
            End If
        End If
        Return state
    End Function

    Public Function IsChanged([property] As PropertyInfo, ByVal target As T) As Boolean
        Dim oValue As Object
        Dim tValue As Object
        Dim changed As Boolean = False

        oValue = [property].GetValue(Me.Original)
        tValue = [property].GetValue(target)
        If oValue IsNot tValue Then
            If oValue Is Nothing OrElse tValue Is Nothing Then
                changed = True
            ElseIf [property].PropertyType.Name = "Nullable`1" AndAlso [property].PropertyType.GenericTypeArguments.Length > 0 Then
                If IsNullableChanged([property].PropertyType, oValue, tValue) Then
                    changed = True
                End If
            ElseIf [property].PropertyType.Equals(GetType(Byte())) Then
                If IsByteArrayChanged(CType(oValue, Byte()), CType(tValue, Byte())) Then
                    changed = True
                End If
            ElseIf oValue.ToString <> tValue.ToString Then
                changed = True
            End If
        End If
        Return changed
    End Function

    Public Function IsNullableChanged(ByVal [type] As Type, oValue As Object, ByVal tValue As Object) As Boolean
        Dim oHasValue As Boolean
        Dim tHasValue As Boolean
        Dim valueInfo As PropertyInfo
        Dim changed As Boolean = False
        Dim hasValue As PropertyInfo = [type].GetProperties(BindingFlags.Instance Or BindingFlags.Public) _
            .Where(Function(p As PropertyInfo) p.CanRead AndAlso p.Name = "HasValue" AndAlso p.PropertyType.Equals(GetType(Boolean))) _
            .First()

        oHasValue = CType(hasValue.GetValue(oValue), Boolean)
        tHasValue = CType(hasValue.GetValue(tValue), Boolean)

        If oHasValue <> tHasValue Then
            changed = True
        ElseIf oHasValue Then
            valueInfo = [type].GetProperties(BindingFlags.Instance Or BindingFlags.Public) _
            .Where(Function(p As PropertyInfo) p.CanRead AndAlso p.Name = "Value") _
            .First()

            oValue = valueInfo.GetValue(oValue)
            tValue = valueInfo.GetValue(tValue)
            If oValue.ToString() <> tValue.ToString() Then
                changed = True
            End If
        End If

        Return changed
    End Function

    Public Function IsByteArrayChanged(oValue As Byte(), ByVal tValue As Byte()) As Boolean
        Dim changed As Boolean = False
        Dim i As Integer

        If oValue IsNot Nothing AndAlso oValue IsNot tValue Then
            If oValue.Length <> tValue.Length Then
                changed = True
            Else
                i = 0
                While changed = False AndAlso i < oValue.Length
                    If oValue(i) <> tValue(i) Then
                        changed = True
                    End If
                    i += 1
                End While
            End If
        End If
        Return changed
    End Function

    Private Function GetProperties(ByVal [type] As Type) As IEnumerable(Of PropertyInfo)
        Return From p In [type].GetProperties(BindingFlags.Instance Or BindingFlags.Public)
               Where p.CanRead = True AndAlso p.GetCustomAttributes(Of ColumnMappingAttribute)(True).Any()
    End Function
End Class
