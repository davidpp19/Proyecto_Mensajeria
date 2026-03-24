using API.Consumer;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mensajeria.MVC.Controllers
{
    public class EstadoMensajesController : Controller
    {
        // GET: EstadoMensajesController
        public ActionResult Index()
        {
            var estadoMensajes = CRUD<EstadoMensaje>.GetAll();
            return View(estadoMensajes);
        }

        // GET: EstadoMensajesController/Details/5
        public ActionResult Details(int id)
        {
            var estadoMensaje = CRUD<EstadoMensaje>.GetById(id);
            if(estadoMensaje == null)
            {
                return NotFound();
            }
            return View(estadoMensaje);
        }

        // GET: EstadoMensajesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstadoMensajesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EstadoMensaje estadoMensaje)
        {
            try
            {
                CRUD<EstadoMensaje>.Create(estadoMensaje);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(estadoMensaje);
            }
        }

        // GET: EstadoMensajesController/Edit/5
        public ActionResult Edit(int id)
        {
            var estadoMensaje = CRUD<EstadoMensaje>.GetById(id);
            if(estadoMensaje == null)
            {
                return NotFound();
            }
            return View(estadoMensaje);
        }

        // POST: EstadoMensajesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EstadoMensaje estadoMensaje)
        {
            try
            {
                CRUD<EstadoMensaje>.Update(id, estadoMensaje);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(estadoMensaje);
            }
        }

        // GET: EstadoMensajesController/Delete/5
        public ActionResult Delete(int id)
        {
            var estadoMensaje = CRUD<EstadoMensaje>.GetById(id);
            if(estadoMensaje == null)
            {
                return NotFound();
            }
            return View(estadoMensaje);
        }

        // POST: EstadoMensajesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, EstadoMensaje estadoMensaje)
        {
            try
            {
                CRUD<EstadoMensaje>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(estadoMensaje);
            }
        }
    }
}
