using Microsoft.EntityFrameworkCore;
using PracticaNetCoreLibros.Models;

namespace PracticaNetCoreLibros.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public DbSet<Libro> Libros { get; set; }

        public DbSet<Genero> Generos { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<VistaPedido> VistaPedido { get; set; }
    }
}
