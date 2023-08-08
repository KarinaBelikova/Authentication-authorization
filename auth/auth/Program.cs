using System.Configuration;
using auth.Data;
using auth.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<UserContext>(
    options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(options => options
    .WithOrigins(new []{"http://localhost:3000", "http://localhost:8080", "http://localhost:4200"})
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

