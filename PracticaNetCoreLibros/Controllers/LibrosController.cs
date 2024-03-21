using Microsoft.AspNetCore.Mvc;
using PracticaNetCoreLibros.Extensions;
using PracticaNetCoreLibros.Filters;
using PracticaNetCoreLibros.Models;
using PracticaNetCoreLibros.Repositories;

namespace PracticaNetCoreLibros.Controllers
{
    public class LibrosController : Controller
    {

        private LibrosRepository repo; 
        public LibrosController(LibrosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Libro> libros = await this.repo.GetLibroAsync();
            return View(libros);
        }

        public async Task<IActionResult> LibrosGenero(int idGenero)
        {
            List<Libro> libros = await this.repo.GetLibrosByGeneroAsync(idGenero);
            return View(libros);
        }

        public async Task<IActionResult> Details(int idLibro)
        {
            Libro libro = await this.repo.FindByIdAsync(idLibro);
            return View(libro);
        }

        public async Task<IActionResult> Carrito(int? idEliminar)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito != null)
            {
                if(idEliminar != null)
                {
                    carrito.Remove(idEliminar.Value);
                    if(carrito.Count == 0)
                    {
                        HttpContext.Session.Remove("CARRITO");
                    } else
                    {
                        HttpContext.Session.SetObject("CARRITO", carrito);
                    }
                }
                List<Libro> libros = await this.repo.GetAllLibrosByIdAsync(carrito);
                return View(libros);
            }
            return View();
        }

        public IActionResult AddCarrito(int idLibro)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if(carrito == null)
            {
                carrito = new List<int>();
                carrito.Add(idLibro);
            }
            else {
                carrito.Add(idLibro);
            }
            HttpContext.Session.SetObject("CARRITO", carrito);
            return RedirectToAction("Carrito");
        }
        [AuthorizeUsers]

        public async Task<IActionResult> comprar()
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            List<Libro> libros = await this.repo.GetAllLibrosByIdAsync(carrito);
            int IdFactura = await this.repo.GetMaxIdFacturaAsync();
            int IdUser = int.Parse(HttpContext.User.FindFirst("ID").Value);
            foreach(Libro libro in libros)
            {
                await this.repo.comprarLibroAsync(libro.IdLibro, IdUser, 1, IdFactura);
            }
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("Pedidos");

        }

        [AuthorizeUsers]
        public async Task<IActionResult> Pedidos()
        {
            List<VistaPedido> listapedidos = await this.repo.GetVistaPedidoAsync();
            return View(listapedidos);
        }

    }
}
