Imports System.Web.Http
Imports System.Web.Http.Description
Imports Swashbuckle.Application
Imports Swashbuckle.Swagger
Public Class SwaggerConfig
    Public Shared Sub Register()
        Register(GlobalConfiguration.Configuration)
    End Sub

    Public Shared Sub Register(ByVal config As HttpConfiguration)
        Dim thisAssembly As Reflection.Assembly = GetType(SwaggerConfig).Assembly()

        config.EnableSwagger(
        Sub(c As SwaggerDocsConfig)
            Dim oauthSchemeBuilder As OAuth2SchemeBuilder

            c.SingleApiVersion("v1", "JestersCreditUnionAPI")
            c.IncludeXmlComments(System.IO.Path.ChangeExtension(thisAssembly.CodeBase, "xml"))
            c.OperationFilter(Of AssignOAuth2SecurityRequirements)()
            oauthSchemeBuilder = c.OAuth2("oauth2")
            With oauthSchemeBuilder
                .Description("OAuth2 Implicit Grant")
                .Flow("implicit")
                .AuthorizationUrl("https://jesterscreditunion-dvlp.auth0.com/authorize")
                .TokenUrl("https://jesterscreditunion-dvlp.auth0.com/oauth/token")
            End With
        End Sub) _
        .EnableSwaggerUi(
        Sub(c As SwaggerUiConfig)
            c.EnableOAuth2Support(
                        clientId:="dZsTT8cN8UROjie0RmjpT9QCROyof3TY",
                        clientSecret:=Nothing,
                        realm:="jesterscreditunion-dvlp.auth0.com",
                        appName:="http://localhost/jesterscreditunion/forms/api",
                        additionalQueryStringParams:=Nothing)
        End Sub)
    End Sub
End Class

Public Class AssignOAuth2SecurityRequirements
    Implements IOperationFilter

    Public Sub Apply(ByVal operation As Operation, ByVal schemaRegistry As SchemaRegistry, ByVal apiDescription As ApiDescription) Implements IOperationFilter.Apply

        ' Determine if the operation has the Authorize attribute
        Dim authorizeAttributes = apiDescription.ActionDescriptor.GetCustomAttributes(Of AuthorizeAttribute)()

        If (authorizeAttributes.Any() = False) Then Exit Sub

        ' Initialize the operation.security property
        If (operation.security Is Nothing) Then
            operation.security = New List(Of IDictionary(Of String, IEnumerable(Of String)))()
        End If

        ' Add the appropriate security definition to the operation
        Dim oAuthRequirements As New Dictionary(Of String, IEnumerable(Of String)) From
        {
            {"oauth2", Enumerable.Empty(Of String)()}
        }

        operation.security.Add(oAuthRequirements)
    End Sub
End Class