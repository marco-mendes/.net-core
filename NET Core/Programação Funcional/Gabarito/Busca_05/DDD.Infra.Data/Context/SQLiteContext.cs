using Microsoft.EntityFrameworkCore;
using DDD.Domain.Entities;
using DDD.Infra.Data.Mapping;

namespace DDD.Infra.Data.Context
{
    public class SQLiteContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Fabricante> Fabricante { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<CategoriaProduto> CategoriaProduto { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Utilizando um servidor SQLite local. Aqui poderíamos configurar qualquer outro banco de dados.
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite("DataSource=app.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(new UserMap().Configure);
            modelBuilder.Entity<Produto>(new ProdutoMap().Configure);
            modelBuilder.Entity<Categoria>(new CategoriaMap().Configure);
            modelBuilder.Entity<Fabricante>(new FabricanteMap().Configure);
            modelBuilder.Entity<CategoriaProduto>(new CategoriaProdutoMap().Configure);
        }
    }
}