﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.CommonAPI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection("CorsOrigins");
            string[] corsOrigins = section.GetChildren().Select<IConfigurationSection, string>(child => child.Value).ToArray();
            if (corsOrigins != null && corsOrigins.Length > 0)
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                        builder.WithOrigins(corsOrigins);
                    });
                });
            }
            return services;
        }

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            string googleIdIssuer = configuration["GoogleIdIssuer"];
            string idIssuer = configuration["IdIssuer"];
            List<string> authenticationSchemes = new List<string>();
            authenticationSchemes.Add(Constants.AUTH_SCHEMA_JCU);
            if (!string.IsNullOrEmpty(googleIdIssuer))
                authenticationSchemes.Add(Constants.AUTH_SCHEME_GOOGLE);
            services.AddAuthorization(o =>
            {
                o.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(authenticationSchemes.ToArray())
                .Build();
                Console.WriteLine($"GoogleIdIssuer={googleIdIssuer}");
                if (!string.IsNullOrEmpty(googleIdIssuer))
                {
                    o.AddPolicy(Constants.POLICY_TOKEN_CREATE,
                        configure =>
                        {
                            configure.AddRequirements(new AuthorizationRequirement(Constants.POLICY_TOKEN_CREATE, googleIdIssuer))
                            .AddAuthenticationSchemes(Constants.AUTH_SCHEME_GOOGLE)
                            .Build();
                        });
                }
                Console.WriteLine($"IdIssuer={idIssuer}");
                if (!string.IsNullOrEmpty(idIssuer))
                {
                    o.AddPolicy(Constants.POLICY_BL_AUTH,
                        configure =>
                        {
                            configure.AddRequirements(new AuthorizationRequirement(Constants.POLICY_BL_AUTH, idIssuer))
                            .AddAuthenticationSchemes(Constants.AUTH_SCHEMA_JCU)
                            .Build();
                        });
                    AddPolicy(o, Constants.POLICY_WORKTASK_TYPE_READ, Constants.AUTH_SCHEMA_JCU, idIssuer);
                    AddPolicy(o, Constants.POLICY_WORKTASK_TYPE_EDIT, Constants.AUTH_SCHEMA_JCU, idIssuer, _workTaskTypeEditPolicies);
                }
            });
            return services;
        }

        private static string[] _workTaskTypeEditPolicies = new string[]
        {
            Constants.POLICY_WORKTASK_TYPE_EDIT,
            Constants.POLICY_WORKTASK_TYPE_READ
        };

        private static void AddPolicy(AuthorizationOptions authorizationOptions, string policyName, string schema, string issuer, IEnumerable<string> additinalPolicies = null)
        {
            if (additinalPolicies == null)
            {
                additinalPolicies = new List<string> { policyName };
            }
            else if (!additinalPolicies.Contains(policyName))
            {
                additinalPolicies = additinalPolicies.Concat(new List<string> { policyName });
            }
            authorizationOptions.AddPolicy(policyName,
                    configure =>
                    {
                        configure.AddRequirements(new AuthorizationRequirement(policyName, issuer, additinalPolicies.ToArray()))
                        .AddAuthenticationSchemes(schema)
                        .Build();
                    });
        }

        public static AuthenticationBuilder AddAuthentication(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            HttpDocumentRetriever documentRetriever = new HttpDocumentRetriever() { RequireHttps = false };
            JsonWebKeySet keySet = JsonWebKeySet.Create(
                documentRetriever.GetDocumentAsync(configuration["JwkAddress"], new System.Threading.CancellationToken()).Result
                );
            builder.AddJwtBearer(Constants.AUTH_SCHEMA_JCU, o =>
            {
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateActor = false,
                    ValidateTokenReplay = false,
                    RequireAudience = false,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ValidAudience = configuration["IdIssuer"],
                    ValidIssuer = configuration["IdIssuer"],
                    IssuerSigningKeys = keySet.GetSigningKeys(),
                    TryAllIssuerSigningKeys = true
                };
                o.IncludeErrorDetails = true;
            })
            ;
            return builder;
        }

        public static AuthenticationBuilder AddGoogleAuthentication(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            HttpDocumentRetriever documentRetriever = new HttpDocumentRetriever() { RequireHttps = false };
            JsonWebKeySet keySet = JsonWebKeySet.Create(
                documentRetriever.GetDocumentAsync(configuration["GoogleJwksUrl"], new System.Threading.CancellationToken()).Result
                );
            builder.AddJwtBearer(Constants.AUTH_SCHEME_GOOGLE, o =>
            {
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateActor = false,
                    ValidateTokenReplay = false,
                    RequireAudience = false,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ValidAudience = configuration["GoogleIdAudience"],
                    ValidIssuer = configuration["GoogleIdIssuer"],
                    IssuerSigningKeys = keySet.GetSigningKeys(),
                    TryAllIssuerSigningKeys = true
                };
                o.IncludeErrorDetails = true;
            })
            ;
            return builder;
        }
    }
}
