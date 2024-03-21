using Microsoft.AspNetCore.Mvc;
using PracticaNetCoreLibros.Filters;
using PracticaNetCoreLibros.Repositories;

namespace PracticaNetCoreLibros.Controllers
{
    public class UsuariosController : Controller
    {
        private LibrosRepository repo;
        public UsuariosController(LibrosRepository repo)
        {
            this.repo = repo;
        }

        [AuthorizeUsers]
        public IActionResult Perfil()
        {
            return View();
        }
    }
}
