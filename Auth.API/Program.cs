using Auth.API.Extensions;
using Auth.API.Interfaces;
using Auth.API.Repositories;
using Auth.API.Services;
using Auth.API.Validations;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI opcional
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddTransient<UsuarioValidation>();

builder.Services.AddControllers();

// DbContext
builder.Services.AddCustomDbContext(builder.Configuration);
// Swagger
builder.Services.AddCustomSwagger();
// Jwt Token
builder.Services.AddCustomTokenJwt(builder.Configuration);

var app = builder.Build();

// Middleware global de tratamento de erros
app.UseCustomExceptionHandler(app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>());

//Jwt
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();