using Estoque.API.Extensions;
using Estoque.API.Interfaces;
using Estoque.API.Messaging;
using Estoque.API.Services;
using Estoque.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI opcional
builder.Services.AddOpenApi();

builder.Services.AddScoped<IPedidoMessageConsumer, PedidoMessageConsumer>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// RabbitMQ
builder.Services.AddScoped<PedidoMessageConsumer>();

// DbContext
builder.Services.AddCustomDbContext(builder.Configuration);
// Swagger
builder.Services.AddCustomSwagger();
// Jwt Token
builder.Services.AddCustomTokenJwt(builder.Configuration);

// Adiciona suporte a controllers
builder.Services.AddControllers();

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

// Mapeia os controllers
app.MapControllers();

// Inicializa o consumer RabbitMQ
using (var scope = app.Services.CreateScope())
{
    var consumer = scope.ServiceProvider.GetRequiredService<IPedidoMessageConsumer>();
    await consumer.ConsumeMessagesAsync();
}

app.Run();