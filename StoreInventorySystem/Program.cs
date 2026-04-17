using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using StoreInventorySystem.Application;
using StoreInventorySystem.Infrastructure;
using StoreInventorySystem.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=products.db"));
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseLogMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
