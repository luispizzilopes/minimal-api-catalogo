using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo.ApiEndPoints;
using MinimalApiCatalogo.Context;
using MinimalApiCatalogo.Models;
using System.Reflection.Metadata.Ecma335;

namespace MinimalApiCatalogo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection,
            ServerVersion.AutoDetect(mySqlConnection)));

        var app = builder.Build();

        app.MapCategoriasEndPoint();
        app.MapProdutoEndPoint();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection(); 

        app.UseAuthorization();

        app.Run();
    }
}