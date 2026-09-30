using API_Estoque.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DbEstoqueContext>(o => o.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=dbEstoque;Trusted_Connection=True;TrustServerCertificate=True"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "swagger"));

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
