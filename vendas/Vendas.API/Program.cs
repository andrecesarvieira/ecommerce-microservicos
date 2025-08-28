
using Microsoft.EntityFrameworkCore;
using Vendas.API.Data;
using Vendas.API.Interfaces;
using Vendas.API.Messaging;
using Vendas.API.Repositories;
using Vendas.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IPedidoMessagePublisher, PedidoMessagePublisher>();
builder.Services.AddScoped<IPedidoMessagePublisher, PedidoMessagePublisher>();
builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoValidator, PedidoValidator>();

// Adiciona o publisher RabbitMQ
builder.Services.AddScoped<PedidoMessagePublisher>();

// Adiciona suporte a controllers com enums como string
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// OpenAPI opcional
builder.Services.AddOpenApi();

// DbContext
builder.Services.AddDbContext<VendasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

var app = builder.Build();

// Middleware global de tratamento de erros
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null)
        {
            var ex = error.Error;
            await context.Response.WriteAsync($"{{\"erro\":\"{ex.Message}\"}}");
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();