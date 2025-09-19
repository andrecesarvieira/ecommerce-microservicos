using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Estoque.API.Extensions;

public static class TokenJwtExtensions
{
    public static void AddCustomTokenJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"];
        var keyBytes = Encoding.UTF8.GetBytes(jwtKey!);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });
        services.AddAuthorizationBuilder()
            .AddPolicy("Administrador", policy => policy.RequireRole("Administrador"))
            .AddPolicy("Estoquista", policy => policy.RequireRole("Estoquista"))
            .AddPolicy("Vendedor", policy => policy.RequireRole("Vendedor"));
    }
}
