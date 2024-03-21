using Microsoft.AspNetCore.Mvc;
using PracticaNetCoreLibros.Models;
using PracticaNetCoreLibros.Repositories;

namespace PracticaNetCoreLibros.ViewComponents
{
    public class MenuGenerosViewComponent: ViewComponent
    {
        private LibrosRepository repo;
        public MenuGenerosViewComponent(LibrosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
