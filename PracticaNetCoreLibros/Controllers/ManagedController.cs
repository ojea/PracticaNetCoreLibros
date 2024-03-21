using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PracticaNetCoreLibros.Repositories;
using PracticaNetCoreLibros.Models;

namespace PracticaNetCoreLibros.Controllers
{


    public class ManagedController : Controller

    {
        private LibrosRepository repo;

        public ManagedController(LibrosRepository repo)

        {

            this.repo = repo;

        }
        public IActionResult Login()

        {

            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Login

            (string email, string password)
        {
            Usuario user = await
                this.repo.LoginUsuariosAsync(email, password);
            if (user != null)
            {
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);

                Claim claimName = new Claim(ClaimTypes.Name, user.Nombre);
                Claim claimRole = new Claim(ClaimTypes.Role, ("USER"));
                Claim claimEmail = new Claim(ClaimTypes.Email, user.Email);
                Claim claimApellidos = new Claim(ClaimTypes.Surname, user.Apellidos);
                Claim claimFoto = new Claim("FOTO", user.Foto);
                Claim claimId = new Claim("ID", user.IdUsuario.ToString());
                identity.AddClaim(claimName);
                identity.AddClaim(claimRole);
                identity.AddClaim(claimEmail);
                identity.AddClaim(claimApellidos);
                identity.AddClaim(claimFoto);
                identity.AddClaim(claimId);

                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);

                TempData["BIENVENIDO"] = "Bienvenido " + user.Nombre;
                return RedirectToAction("Perfil", "Usuarios");

            } else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }

        }

        public async Task<IActionResult> Logout()

        {

            await HttpContext.SignOutAsync

                (CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");

        }

    }
}
