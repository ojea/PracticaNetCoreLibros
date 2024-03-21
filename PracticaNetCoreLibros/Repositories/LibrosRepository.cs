

using Microsoft.EntityFrameworkCore;
using PracticaNetCoreLibros.Data;
using PracticaNetCoreLibros.Models;

namespace PracticaNetCoreLibros.Repositories
{
    public class LibrosRepository
    {
        private ApplicationDbContext context;

        public LibrosRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }

        public async Task<List<Libro>> GetLibroAsync()
        {
            return await this.context.Libros.ToListAsync();

        }

        public async Task<Libro> FindByIdAsync(int idLibro)
        {
            return await this.context.Libros.FirstOrDefaultAsync(x => x.IdLibro == idLibro);
        }

        public async Task<List<Libro>> GetLibrosByGeneroAsync(int idGenero)
        {
            return await this.context.Libros.Where(x => x.IdGenero == idGenero).ToListAsync();
        }

        public async Task<List<Libro>> GetAllLibrosByIdAsync(List<int> listIdLibros)
        {
            var consulta = from datos in this.context.Libros
                          where listIdLibros.Contains(datos.IdLibro)
                          select datos;
            if (consulta.Count() == 0)
                return null;
            return await consulta.ToListAsync();
        }

        public async Task<Usuario>LoginUsuariosAsync(string email, string password)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email && x.Pass == password);
        }

        public async Task<int> GetMaxIdPedidosAsyn()
        {
            return await this.context.Pedidos.MaxAsync(x => x.IdPedidos) + 1;
        }

        public async Task<int> GetMaxIdFacturaAsync()
        {
            return (int)(await this.context.Pedidos.MaxAsync(x => x.IdFactura) + 1);
        }

        public async Task<List<VistaPedido>> GetVistaPedidoAsync()
        {
            return await this.context.VistaPedido.ToListAsync();
        }

        public async Task comprarLibroAsync(int idLibro, int idUsuario, int cantidad, int idFactura)
        {
            Pedido pedido = new Pedido
            {
                Cantidad = cantidad,
                Fecha = DateTime.Now,
                IdFactura = idFactura,
                IdLibro = idLibro,
                IdPedidos = await this.GetMaxIdPedidosAsyn(),
                IdUsuario = idUsuario
            };
            this.context.Add(pedido);
            await this.context.SaveChangesAsync();
        }


    }
}
