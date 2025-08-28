using Estoque.API.Interfaces;
using Estoque.API.Messaging;
using Estoque.API.Data;
using Microsoft.EntityFrameworkCore;
using Estoque.API.Services;
using Estoque.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPedidoMessageConsumer, PedidoMessageConsumer>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// Adiciona o consumer RabbitMQ
builder.Services.AddScoped<PedidoMessageConsumer>();

// Adiciona suporte a controllers
builder.Services.AddControllers();

// OpenAPI opcional
builder.Services.AddOpenApi();

// DbContext
builder.Services.AddDbContext<EstoqueContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Mapeia os controllers
app.MapControllers();

// Inicializa o consumer RabbitMQ
using (var scope = app.Services.CreateScope())
{
    var consumer = scope.ServiceProvider.GetRequiredService<IPedidoMessageConsumer>();
    await consumer.ConsumeMessagesAsync();
}

app.Run();