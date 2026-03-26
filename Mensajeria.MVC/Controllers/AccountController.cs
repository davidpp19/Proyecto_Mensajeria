using API.Consumer;
using Mensajeria.Servicios.Interfaces;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Mensajeria.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Account
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            email = email.Trim().ToLower();

            if (await _authService.Login(email, password))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Email o contraseña incorrectos.";
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            // Mostrar opción de crear administrador sólo si no existe ningún administrador
            var usuarios = CRUD<Usuario>.GetAll();
            bool existeAdmin = usuarios.Any(u => u.RolId == 1);
            ViewBag.CanCreateAdmin = !existeAdmin;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string nombreUsuario, string email, string password, int? rolId, bool crearAdmin = false)
        {
            email = email.Trim().ToLower();

            var usuario = CRUD<Usuario>.GetAll()
                .FirstOrDefault(u => u.Correo_Usuario.ToLower() == email);

            if (usuario != null)
            {
                ViewBag.ErrorMessage = "Esta cuenta ya está asociada a este correo";
                return View();
            }

            // Si el usuario marca crearAdmin y no existe admin, asignar rol admin
            if (crearAdmin)
            {
                rolId = 1; // Administrador
            }

            if (await _authService.Register(nombreUsuario, email, password, rolId))
            {
                return RedirectToAction("Index", "Account");
            }

            ViewBag.ErrorMessage = "Error al crear el usuario";
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            // Elimina la cookie de autenticación
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Account");
        }


    }
}