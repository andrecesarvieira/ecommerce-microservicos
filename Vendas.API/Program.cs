using Vendas.API.Extensions;
using Vendas.API.Interfaces;
using Vendas.API.Messaging;
using Vendas.API.Repositories;
using Vendas.API.Services;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI opcional
builder.Services.AddOpenApi();

builder.Services.AddScoped<IPedidoMessagePublisher, PedidoMessagePublisher>();
builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoValidator, PedidoValidator>();

// RabbitMQ
builder.Services.AddScoped<PedidoMessagePublisher>();

// DbContext
builder.Services.AddCustomDbContext(builder.Configuration);
// Swagger
builder.Services.AddCustomSwagger();
// Jwt Token
builder.Services.AddCustomTokenJwt(builder.Configuration);

// Adiciona suporte a controllers com enums como string
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

var app = builder.Build();

// Middleware global de tratamento de erros
app.UseCustomExceptionHandler(app.Services.GetRequiredService<ILoggerFactory>());

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