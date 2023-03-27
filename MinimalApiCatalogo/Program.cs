using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //definindo os endpoints 
        app.MapPost("/categorias", async ([FromBody] Categoria categoria, [FromServices] AppDbContext db) =>
        {
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();

            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        });

        app.MapGet("/categorias", async ([FromServices] AppDbContext db) =>
        {
            return Results.Ok(await db.Categorias.ToListAsync());
        });

        app.MapGet("/categorias/{id:int}", async (int id, [FromServices] AppDbContext db) =>
        {
            var categoria = await db.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);

            return categoria != null
                ? Results.Ok(categoria)
                : Results.NotFound("Não foi possível localizar uma categoria com o Id informado");
        });

        app.MapPut("categoria/{id:int}", async (int id, [FromBody] Categoria categoria, [FromServices] AppDbContext db) =>
        {
            if (id != categoria.CategoriaId)
            {
                return Results.BadRequest();
            }

            var categoriaDb = await db.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);

            if (categoriaDb == null)
            {
                return Results.NotFound();
            }

            categoriaDb.Nome = categoria.Nome;
            categoriaDb.Descricao = categoria.Descricao;

            await db.SaveChangesAsync();
            return Results.Ok(categoriaDb);
        });

        app.MapDelete("categorias/{id:int}", async (int id, [FromServices] AppDbContext db) =>
        {
            var categoria = await db.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return Results.NotFound();
            }

            db.Remove(categoria);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        //---------------------------PRODUTOS---------------------------------------

        app.MapPost("produtos/", async ([FromBody] Produto produto, [FromServices] AppDbContext db) =>
        {
            db.Produtos.Add(produto);
            await db.SaveChangesAsync();

            return Results.Created($"produtos/{produto.ProdutoId}", produto);
        });

        app.MapGet("produtos/", async ([FromServices] AppDbContext db) =>
        {
            return Results.Ok(await db.Produtos.ToListAsync());
        });

        app.MapGet("produtos/{id:int}", async (int id, [FromServices] AppDbContext db) =>
        {
            var produto = await db.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(produto);
        });

        app.MapPut("produto/{id:int}", async (int id, [FromBody] Produto produto, [FromServices] AppDbContext db) =>
        {
            if (produto.ProdutoId != id)
            {
                return Results.BadRequest();
            }

            var produtoDb = await db.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produtoDb == null)
            {
                return Results.NotFound();
            }

            produtoDb.Nome = produto.Nome;
            produtoDb.Descricao = produto.Descricao;
            produtoDb.Preco = produto.Preco;
            produtoDb.Imagem = produto.Imagem;
            produtoDb.DataCompra = produto.DataCompra;
            produtoDb.Estoque = produto.Estoque;
            produtoDb.CategoriaId = produto.CategoriaId;

            await db.SaveChangesAsync();

            return Results.Ok();
        });

        app.MapDelete("produto/{id:int}", async(int id, [FromServices] AppDbContext db) =>
        {
            var produto = await db.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

            if(produto == null)
            {
                return Results.NotFound(); 
            }

            db.Remove(produto);
            await db.SaveChangesAsync();

            return Results.Ok();
        });

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