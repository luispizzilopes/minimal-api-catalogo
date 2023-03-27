using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo.Context;
using MinimalApiCatalogo.Models;

namespace MinimalApiCatalogo.ApiEndPoints
{
    public static class CategoriaEndPoints
    {
        public static void MapCategoriasEndPoint(this WebApplication app)
        {
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
        }
    }
}
