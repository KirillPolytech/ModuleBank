using BankAccount.Features.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace BankAccount.Services
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddSwaggerGen(
            SwaggerGenOptions options,
            IConfiguration configuration)
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            options.IncludeXmlComments(xmlPath);
            options.EnableAnnotations();

            var authority = configuration["Jwt:Authority"];
            var tokenAuthority = configuration["Jwt:TokenAuthority"];
            var auth = configuration["Jwt:AuthUrl"];

            if (string.IsNullOrEmpty(authority))
                throw new InvalidOperationException("Jwt:Authority configuration is missing.");

            if (string.IsNullOrEmpty(tokenAuthority))
                throw new InvalidOperationException("Jwt:TokenAuthority configuration is missing.");

            if (string.IsNullOrEmpty(auth))
                throw new InvalidOperationException("Jwt:AuthUrl configuration is missing.");

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(auth),
                        TokenUrl = new Uri(tokenAuthority),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID scope" },
                            { "profile", "Profile scope" }
                        }
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    ["openid", "profile"]
                }
            });
        }

        internal static void AddAuthentication(JwtBearerOptions options, IConfiguration configuration)
        {
            options.RequireHttpsMetadata = false;
            options.Audience = "account"; //configuration.Configuration["Jwt:Audience"];

            options.Authority = configuration["Jwt:Authority"];

            options.MetadataAddress = configuration["Jwt:MetadataAddress"]!;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:ValidIssuer"],
            };
        }

        internal static void ConfigureApiBehaviorOptions(ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var error = context.ModelState
                    .SelectMany(kvp => kvp.Value?.Errors ?? [])
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault() ?? "Invalid request";

                return new JsonResult(MbResult<object>.Fail(error))
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            };
        }

        internal static void AddCors(CorsOptions options)
        {
            options.AddPolicy("AllowAll",
                policy => policy.WithOrigins("http://localhost", "http://localhost:5000")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        }
    }
}