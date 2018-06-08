Imports System.IO
Imports System.Reflection
Public Class TransformIndex
    Private Shared resourceMan As Global.System.Resources.ResourceManager

    Private Shared resourceCulture As Global.System.Globalization.CultureInfo

    <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    Friend Sub New()
        MyBase.New
    End Sub

    '''<summary>
    '''  Returns the cached ResourceManager instance used by this class.
    '''</summary>
    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>
    Public Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
        Get
            If Object.ReferenceEquals(resourceMan, Nothing) Then
                Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("JestersCreditUnion.Core.Form.Style.Index", GetAssembly)
                resourceMan = temp
            End If
            Return resourceMan
        End Get
    End Property

    '''<summary>
    '''  Overrides the current thread's CurrentUICulture property for all
    '''  resource lookups using this strongly typed resource class.
    '''</summary>
    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>
    Public Shared Property Culture() As Global.System.Globalization.CultureInfo
        Get
            Return resourceCulture
        End Get
        Set
            resourceCulture = Value
        End Set
    End Property

    '''<summary>
    '''  Looks up a localized string similar to RoleRequest-1.xslt.
    '''</summary>
    Public Shared ReadOnly Property RoleRequest1() As String
        Get
            Return ResourceManager.GetString("RoleRequest1", resourceCulture)
        End Get
    End Property

    Private Shared Function GetAssembly() As Assembly
        Return Assembly.Load("JestersCreditUnion.Core.Form.Style")
    End Function

    Public Shared Function GetResourceStream(ByVal resourceName As String) As Stream
        Dim assembly As Assembly = GetAssembly()

        Return assembly.GetManifestResourceStream(resourceName)
    End Function
End Class
