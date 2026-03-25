using API.Consumer;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mensajeria.MVC.Controllers
{
    public class ParticipantesConversacionController : Controller
    {
        // GET: ParticipantesConversacionController
        public ActionResult Index()
        {
            var participantes = CRUD<ParticipanteConversacion>.GetAll();
            return View(participantes);
        }

        // GET: ParticipantesConversacionController/Details/5
        public ActionResult Details(int conversacionId, int usuarioId)
        {
            var participante = CRUD<ParticipanteConversacion>.GetById(conversacionId, usuarioId);
            if(participante == null)
            {
                return NotFound();
            }
            return View(participante);
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
        private List<SelectListItem> GetUsuarios()
        {
            var usuarios = CRUD<Usuario>.GetAll();
            return usuarios.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),

                Text = b.Nombre_Usuario
            }).ToList();
        }
        

        // GET: ParticipantesConversacionController/Create
        public ActionResult Create()
        {
            ViewBag.Conversaciones = GetConversaciones();
            ViewBag.Usuarios = GetUsuarios();
            return View();
        }

        // POST: ParticipantesConversacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ParticipanteConversacion participante)
        {
            try
            {
                CRUD<ParticipanteConversacion>.Create(participante);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(participante);
            }
        }

        // GET: ParticipantesConversacionController/Edit/5
        public ActionResult Edit(int conversacionId, int usuarioId)
        {
            var participante = CRUD<ParticipanteConversacion>.GetById(conversacionId, usuarioId);
            ViewBag.Conversaciones = GetConversaciones();
            ViewBag.Usuarios = GetUsuarios();
            if(participante == null)
            {
                return NotFound();
            }
            return View(participante);
        }

        // POST: ParticipantesConversacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int conversacionId, int usuarioId, ParticipanteConversacion participante)
        {
            try
            {
                CRUD<ParticipanteConversacion>.Update(conversacionId, usuarioId, participante);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(participante);
            }
        }

        // GET: ParticipantesConversacionController/Delete/5
        public ActionResult Delete(int conversacionId, int usuarioId)
        {
            var participante = CRUD<ParticipanteConversacion>.GetById(conversacionId, usuarioId);
            if(participante == null)
            {
                return NotFound();
            }
            return View(participante);
        }

        // POST: ParticipantesConversacionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int conversacionId, int usuarioId, ParticipanteConversacion participante)
        {
            try
            {
                CRUD<ParticipanteConversacion>.Delete(conversacionId, usuarioId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(participante);
            }
        }
    }
}
