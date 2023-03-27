using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo.Models;

namespace MinimalApiCatalogo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configurando as propriedades da entidade categoria
            modelBuilder.Entity<Categoria>().HasKey(c => c.CategoriaId);
            modelBuilder.Entity<Categoria>().Property(c => c.Nome).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Categoria>().Property(c => c.Descricao).HasMaxLength(100).IsRequired();

            //Configurnado as propriedades da entidade produto
            modelBuilder.Entity<Produto>().HasKey(c => c.ProdutoId);
            modelBuilder.Entity<Produto>().Property(c => c.Nome).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Produto>().Property(c => c.Descricao).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Produto>().Property(c => c.Imagem).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Produto>().Property(c => c.Preco).HasPrecision(14, 2); 

            //Relacionamente entre as entidades
            modelBuilder.Entity<Produto>()
                .HasOne<Categoria>(c => c.Categoria)
                .WithMany(p => p.Produtos)
                .HasForeignKey(c => c.CategoriaId);

        }
        
    }
}
