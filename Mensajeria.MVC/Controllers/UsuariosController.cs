using API.Consumer;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mensajeria.MVC.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: UsuariosController
        public ActionResult Index()
        {
            var usuario = CRUD<Usuario>.GetAll();
            return View(usuario);
        }

        // GET: UsuariosController/Details/5
        public ActionResult Details(int id)
        {
            var usuario = CRUD<Usuario>.GetById(id);
            if(usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        //Método interno para obtener los roles, es un GET ROLES
        private List<SelectListItem> GetRoles()
        {
            var roles = CRUD<Rol>.GetAll();
            return roles.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Nombre_Rol
            }).ToList();
        }

        // GET: UsuariosController/Create
        public ActionResult Create()
        {
            ViewBag.Roles = GetRoles();
            return View();
        }

        // POST: UsuariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            try
            {
                CRUD<Usuario>.Create(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }

        // GET: UsuariosController/Edit/5
        public ActionResult Edit(int id)
        {
            var usuario = CRUD<Usuario>.GetById(id);
            ViewBag.Roles = GetRoles();
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: UsuariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario usuario)
        {
            try
            {
                CRUD<Usuario>.Update(id, usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }

        // GET: UsuariosController/Delete/5
        public ActionResult Delete(int id)
        {
            var usuario = CRUD<Usuario>.GetById(id);
            if(usuario == null)
            {
                return NotFound();
            }   
            return View(usuario);
        }

        // POST: UsuariosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                CRUD<Usuario>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }
    }
}
