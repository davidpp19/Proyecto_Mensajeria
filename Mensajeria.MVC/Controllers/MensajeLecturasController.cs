using API.Consumer;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mensajeria.MVC.Controllers
{
    public class MensajeLecturasController : Controller
    {
        // GET: MensajeLecturasController
        public ActionResult Index()
        {
            var mensajeLecturas = CRUD<MensajeLectura>.GetAll();
            return View(mensajeLecturas);
        }

        // GET: MensajeLecturasController/Details/5
        public ActionResult Details(int id, int id2)
        {
            var mensajeLectura = CRUD<MensajeLectura>.GetById(id, id2);
            if(mensajeLectura == null)
            {
                return NotFound();
            }
            return View(mensajeLectura);
        }

        //Método interno para obtener el estado del mensaje
        private List<SelectListItem> GetEstado()
        {
            var estado = CRUD<EstadoMensaje>.GetAll();
            return estado.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),

                Text = b.Descripcion_Estado
            }).ToList();
        }

        //Método interno para obtener el usuario del mensaje
        private List<SelectListItem> GetUsuario()
        {
            var usuario = CRUD<Usuario>.GetAll();
            return usuario.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),

                Text = b.Nombre_Usuario
            }).ToList();
        }

        //Método interno para obtener los mensajes.
        private List<SelectListItem> GetMensaje()
        {
            var mensajes = CRUD<Mensaje>.GetAll();
            return mensajes.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),

                Text = b.Contenido_Mensaje
            }).ToList();
        }

        // GET: MensajeLecturasController/Create
        public ActionResult Create()
        {
            ViewBag.Estado = GetEstado();
            ViewBag.Usuarios = GetUsuario();
            ViewBag.Mensajes = GetMensaje();
            return View();
        }

        // POST: MensajeLecturasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MensajeLectura mensajeLectura)
        {
            try
            {
                CRUD<MensajeLectura>.Create(mensajeLectura);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(mensajeLectura);
            }
        }

        // GET: MensajeLecturasController/Edit/5
        public ActionResult Edit(int id, int id2)
        {
            ViewBag.Estado = GetEstado();
            ViewBag.Usuarios = GetUsuario();
            ViewBag.Mensajes = GetMensaje();
            var mensajeLectura = CRUD<MensajeLectura>.GetById(id, id2);
            if(mensajeLectura == null)
            {
                return NotFound();
            }
            return View(mensajeLectura);
        }

        // POST: MensajeLecturasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MensajeLectura mensajeLectura)
        {
            int id = mensajeLectura.MensajeId;
            int id2 = mensajeLectura.UsuarioId;
            try
            {
                CRUD<MensajeLectura>.Update(id, id2, mensajeLectura);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(mensajeLectura);
            }
        }

        // GET: MensajeLecturasController/Delete/5
        public ActionResult Delete(int id, int id2)
        {
            var mensajeLectura = CRUD<MensajeLectura>.GetById(id, id2);
            if(mensajeLectura == null)
            {
                return NotFound();
            }
            return View(mensajeLectura);
        }

        // POST: MensajeLecturasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(MensajeLectura mensajeLectura)
        {
            try
            {
                int idPrincipal = mensajeLectura.MensajeId;
                int idSecundario = mensajeLectura.UsuarioId;

                CRUD<MensajeLectura>.Delete(idPrincipal, idSecundario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(mensajeLectura);
            }
        }
    }
}
