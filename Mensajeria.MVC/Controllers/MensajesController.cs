using API.Consumer;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mensajeria.MVC.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
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

        // GET: api/Mensajes/ByConversacion/5
        [HttpGet]
        public IActionResult ByConversacion(int conversacionId)
        {
            try
            {
                var mensajes = CRUD<Mensaje>.GetAll()
                    .Where(m => m.ConversacionId == conversacionId)
                    .OrderBy(m => m.Id)
                    .ToList();

                var lecturas = CRUD<MensajeLectura>.GetAll();
                var estados = CRUD<EstadoMensaje>.GetAll();
                var usuarios = CRUD<Usuario>.GetAll();

                var result = mensajes.Select(m =>
                {
                    // obtener lecturas del mensaje
                    var lect = lecturas.Where(l => l.MensajeId == m.Id).ToList();

                    string status = "Enviado";
                    if (lect.Any())
                    {
                        // evaluar prioridades: Leído > Entregado > Enviado
                        var descripcionList = lect
                            .Select(l => estados.FirstOrDefault(e => e.Id == l.EstadoMensajeId)?.Descripcion_Estado?.ToLower() ?? string.Empty)
                            .ToList();

                        if (descripcionList.Any(d => d.Contains("le"))) status = "Leído";
                        else if (descripcionList.Any(d => d.Contains("entreg"))) status = "Entregado";
                        else if (descripcionList.Any(d => d.Contains("envi"))) status = "Enviado";
                        else status = descripcionList.FirstOrDefault() ?? "Enviado";
                    }

                    var remitente = usuarios.FirstOrDefault(u => u.Id == m.RemitenteId);

                    return new
                    {
                        id = m.Id,
                        contenido_Mensaje = m.Contenido_Mensaje,
                        hora_Envio_Mensaje = m.Hora_Envio_Mensaje.ToString(),
                        remitente = remitente == null ? null : new { id = remitente.Id, nombre_Usuario = remitente.Nombre_Usuario },
                        estado = status
                    };
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Mensajes/Create
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult CreateMessage([FromBody] Mensaje mensaje)
        {
            try
            {
                mensaje.Hora_Envio_Mensaje = TimeOnly.FromDateTime(DateTime.Now);
                var created = CRUD<Mensaje>.Create(mensaje);
                // Crear estados de lectura asociados
                try
                {
                    var estados = CRUD<EstadoMensaje>.GetAll();
                    var enviadoId = estados.FirstOrDefault(e => e.Descripcion_Estado != null && e.Descripcion_Estado.ToLower().Contains("envi"))?.Id
                        ?? estados.FirstOrDefault()?.Id ?? 1;
                    var entregadoId = estados.FirstOrDefault(e => e.Descripcion_Estado != null && e.Descripcion_Estado.ToLower().Contains("entreg"))?.Id
                        ?? enviadoId;
                    var leidoId = estados.FirstOrDefault(e => e.Descripcion_Estado != null && e.Descripcion_Estado.ToLower().Contains("le"))?.Id
                        ?? enviadoId;

                    // obtener participantes de la conversacion
                    var participantes = CRUD<ParticipanteConversacion>.GetAll()
                        .Where(p => p.ConversacionId == mensaje.ConversacionId)
                        .ToList();

                    // crear MensajeLectura para cada participante
                    foreach (var p in participantes)
                    {
                        var ml = new MensajeLectura
                        {
                            MensajeId = created.Id,
                            UsuarioId = p.UsuarioId,
                            Fecha_Lectura = DateTime.Now,
                            EstadoMensajeId = (p.UsuarioId == mensaje.RemitenteId) ? enviadoId : entregadoId
                        };
                        CRUD<MensajeLectura>.Create(ml);
                    }
                }
                catch { }

                return Json(new { success = true, message = created });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
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
