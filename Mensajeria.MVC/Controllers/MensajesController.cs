using API.Consumer;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mensajeria.MVC.Controllers
{
    public class MensajesController : Controller
    {
        // GET: MensajesController
        public ActionResult Index()
        {
            var mensaje = CRUD<Mensaje>.GetAll();
            return View(mensaje);
        }

        // GET: MensajesController/Details/5
        public ActionResult Details(int id)
        {
            var mensaje = CRUD<Mensaje>.GetById(id);
            if(mensaje == null)
            {
                return NotFound();
            }
            return View(mensaje);
        }
        //Método interno para determinar el tipo de conversación, dependiendo del valor booleano.
        private string TipoConversacion(Conversacion conversacion)
        {
            if (conversacion.Tipo_Conversacion)
            {
                return "Grupal";

            }
            else
            {
                return "Individual";
            }
        }
        //Método interno para obtener las conversaciones, es un GET CONVERSACIONES.
        private List<SelectListItem> GetConversaciones()
        {
            var conversacion = CRUD<Conversacion>.GetAll();
            return conversacion.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                
                Text = TipoConversacion(b)
            }).ToList();
        }

        //Método interno para obtener los remitentes, es un GET REMITENTES.
        private List<SelectListItem> GetRemitentes()
        {
            var remitentes = CRUD<Usuario>.GetAll();
            return remitentes.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),

                Text = b.Nombre_Usuario
            }).ToList();
        }

        // GET: MensajesController/Create
        public ActionResult Create()
        {
            ViewBag.Conversaciones = GetConversaciones();
            ViewBag.Remitentes = GetRemitentes();
            return View();
        }

        // POST: MensajesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Mensaje mensaje)
        {
            try
            {
                CRUD<Mensaje>.Create(mensaje);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(mensaje);
            }
        }

        // GET: MensajesController/Edit/5
        public ActionResult Edit(int id)
        {
            var mensaje = CRUD<Mensaje>.GetById(id);
            ViewBag.Conversaciones = GetConversaciones();
            ViewBag.Remitentes = GetRemitentes();

            if (mensaje == null)
            {
                return NotFound();
            }
            return View(mensaje);
        }

        // POST: MensajesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Mensaje mensaje)
        {
            try
            {
                CRUD<Mensaje>.Update(id, mensaje);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(mensaje);
            }
        }

        // GET: MensajesController/Delete/5
        public ActionResult Delete(int id)
        {
            var mensaje = CRUD<Mensaje>.GetById(id);
            if(mensaje == null)
            {
                return NotFound();
            }
            return View(mensaje);
        }

        // POST: MensajesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Mensaje mensaje)
        {
            try
            {
                CRUD<Mensaje>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(mensaje);
            }
        }
    }
}
