Imports JestersCreditUnion.Interface.AbyssalDataProcessor

Public Class SettingsAdp
    Implements JestersCreditUnion.Interface.AbyssalDataProcessor.ISettings

    Public ReadOnly Property BaseAddress As String Implements ISettings.BaseAddress
        Get
            Return My.Settings.adpBaseAddress
        End Get
    End Property
End Class
