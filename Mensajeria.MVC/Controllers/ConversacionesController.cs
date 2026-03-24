using API.Consumer;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Mensajeria.MVC.Controllers
{
    public class ConversacionesController : Controller
    {
        // GET: ConversacionesController
        public ActionResult Index()
        {
            var conversacion = CRUD<Conversacion>.GetAll();
            return View(conversacion);
        }

        // GET: ConversacionesController/Details/5
        public ActionResult Details(int id)
        {
            var conversacion = CRUD<Conversacion>.GetById(id);
            if(conversacion == null)
            {
                return NotFound();
            }
            return View(conversacion);
        }

        // GET: ConversacionesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConversacionesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Conversacion conversacion)
        {
            try
            {
                CRUD<Conversacion>.Create(conversacion);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(conversacion);
            }
        }

        // GET: ConversacionesController/Edit/5
        public ActionResult Edit(int id)
        {
            var conversacion = CRUD<Conversacion>.GetById(id);
            if (conversacion == null)
            {
                return NotFound();
            }
            return View(conversacion);
        }

        // POST: ConversacionesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Conversacion conversacion)
        {
            try
            {
                CRUD<Conversacion>.Update(id, conversacion);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(conversacion);
            }
        }

        // GET: ConversacionesController/Delete/5
        public ActionResult Delete(int id)
        {
            var conversacion = CRUD<Conversacion>.GetById(id);
            if(conversacion == null)
            {
                return NotFound();
            }
            return View(conversacion);
        }

        // POST: ConversacionesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Conversacion conversacion)
        {
            try
            {
                CRUD<Conversacion>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(conversacion);
            }
        }
    }
}
