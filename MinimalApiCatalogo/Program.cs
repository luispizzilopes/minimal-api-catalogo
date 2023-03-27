using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo.ApiEndPoints;
using MinimalApiCatalogo.AppServicesExtensions;
using MinimalApiCatalogo.Context;
namespace MinimalApiCatalogo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.AddApiSwagger();
        builder.AddPersistence();
        builder.Services.AddCors();

        var app = builder.Build();

        app.MapCategoriasEndPoint();
        app.MapProdutoEndPoint();

        var enviroment = app.Environment;
        app.UseExceptionHandling(enviroment)
            .UseSwaggerMiddleware()
            .UseAppCors(); 

        app.UseHttpsRedirection(); 
        app.UseAuthorization();

        app.Run();
    }
}