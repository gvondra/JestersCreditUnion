Public Class OrganizationSaver
    Implements IOrganizationSaver

    Public Sub Save(settings As ISettings, organization As IOrganization) Implements IOrganizationSaver.Save
        Dim saver As New Saver
        saver.Save(New CoreSettings(settings), AddressOf organization.Save)
    End Sub
End Class
