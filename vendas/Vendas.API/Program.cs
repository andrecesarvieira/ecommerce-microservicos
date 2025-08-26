
using Microsoft.EntityFrameworkCore;
using Vendas.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a controllers
builder.Services.AddControllers();
// OpenAPI opcional
builder.Services.AddOpenApi();
//DbContext
builder.Services.AddDbContext<VendasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Mapeia os controllers
app.MapControllers();

app.Run();
