Imports Auth0.Owin
'Imports Microsoft.IdentityModel.Tokens
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Jwt
Imports Owin
Imports System.IdentityModel.Tokens
Imports System.Web.Http
<Assembly: OwinStartup(GetType(JestersCreditUnionAPIForms.Startup))>
Public Class Startup
    Public Sub Configuration(app As IAppBuilder)
        Dim config As New HttpConfiguration()
        Dim domain As String = $"https://{My.Settings.Auth0Domain}/"
        Dim apiIdentifier As String = My.Settings.Auth0ApiIdentifier

        Dim keyResolver As New OpenIdConnectSigningKeyResolver(domain)
        app.UseJwtBearerAuthentication(New JwtBearerAuthenticationOptions() _
        With {
            .AuthenticationMode = Security.AuthenticationMode.Active,
            .TokenValidationParameters = New TokenValidationParameters() _
            With {
                .SaveSigninToken = True,
                .ValidAudience = apiIdentifier,
                .ValidIssuer = domain,
                .IssuerSigningKeyResolver = Function(token As String, securityToken As System.IdentityModel.Tokens.SecurityToken, kid As SecurityKeyIdentifier, validationParameters As System.IdentityModel.Tokens.TokenValidationParameters) keyResolver.GetSigningKey(kid)
            }
        })

        WebApiConfig.Register(config)
        SwaggerConfig.Register(config)

        app.UseWebApi(config)
    End Sub
End Class
