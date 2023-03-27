using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo.Context;
using MinimalApiCatalogo.Models;

namespace MinimalApiCatalogo.ApiEndPoints
{
    public static class ProdutoEndPoints
    {
        public static void MapProdutoEndPoint(this WebApplication app)
        {
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

            app.MapDelete("produto/{id:int}", async (int id, [FromServices] AppDbContext db) =>
            {
                var produto = await db.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return Results.NotFound();
                }

                db.Remove(produto);
                await db.SaveChangesAsync();

                return Results.Ok();
            });
        }
    }
}
