using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MVCQLCHTHUOC.Models;
using System.Text;
using System;

namespace MVCQLCHTHUOC.Models
{
    public class ApiSecurity
    {
        // Method to configure JWT authentication
        public static void ConfigureJwtAuthentication(IServiceCollection services, string issuer, string audience, string secretKey)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ClockSkew = TimeSpan.Zero // Adjust the expiration tolerance
                    };
                });
        }

        // Method to apply authorization policy for JWT token
        public static void ConfigureJwtAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }

        // Decorator to apply JWT token authorization to controllers or actions
        public class JwtAuthorizeAttribute : AuthorizeAttribute
        {
            public JwtAuthorizeAttribute()
            {
                AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
            }
        }
    }
}

