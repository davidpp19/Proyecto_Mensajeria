using API.Consumer;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mensajeria.MVC.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrador")]
    public class RolesController : Controller
    {
        // GET: RolesController
        public ActionResult Index()
        {
            var rol = CRUD<Rol>.GetAll();
            return View(rol);
        }

        // GET: RolesController/Details/5
        public ActionResult Details(int id)
        {
            var rol = CRUD<Rol>.GetById(id);
            if(rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        // GET: RolesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rol rol)
        {
            try
            {
                CRUD<Rol>.Create(rol);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(rol);
            }
        }

        // GET: RolesController/Edit/5
        public ActionResult Edit(int id)
        {
            var rol = CRUD<Rol>.GetById(id);
            if (rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        // POST: RolesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Rol rol)
        {
            try
            {
                CRUD<Rol>.Update(id, rol);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(rol);
            }
        }

        // GET: RolesController/Delete/5
        public ActionResult Delete(int id)
        {
            var rol = CRUD<Rol>.GetById(id);
            if(rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        // POST: RolesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Rol rol)
        {
            try
            {
                CRUD<Rol>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(rol);
            }
        }
    }
}
