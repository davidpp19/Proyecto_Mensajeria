using API.Consumer;
using Mensajeria.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Mensajeria.MVC.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ConversacionesController : Controller
    {
        // GET: ConversacionesController
        public ActionResult Index()
        {
            var conversacion = CRUD<Conversacion>.GetAll();
            // Enviar lista de usuarios para poder iniciar chat directo
            var usuarios = CRUD<Usuario>.GetAll();
            ViewBag.Usuarios = usuarios;
            // Current user id desde claims
            var currentUserId = 0;
            if (User?.Identity?.IsAuthenticated == true)
            {
                var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (idClaim != null) int.TryParse(idClaim.Value, out currentUserId);
            }
            ViewBag.CurrentUserId = currentUserId;
            // calcular conversaciones con mensajes no leidos para el usuario actual
            try
            {
                var mensajes = CRUD<Mensaje>.GetAll();
                var lecturas = CRUD<MensajeLectura>.GetAll();
                var estados = CRUD<EstadoMensaje>.GetAll();

                var leidoId = estados.FirstOrDefault(e => e.Descripcion_Estado != null && e.Descripcion_Estado.ToLower().Contains("le"))?.Id;

                var unreadIds = new List<int>();
                foreach (var c in conversacion)
                {
                    // mensajes en la conversacion enviados por otros usuarios
                    var msgs = mensajes.Where(m => m.ConversacionId == c.Id && m.RemitenteId != currentUserId).ToList();
                    bool anyUnread = false;
                    foreach (var m in msgs)
                    {
                        // si no existe lectura con estado leido para este usuario y mensaje -> no leido
                        var hasRead = lecturas.Any(l => l.MensajeId == m.Id && l.UsuarioId == currentUserId && (leidoId != null ? l.EstadoMensajeId == leidoId : true));
                        if (!hasRead)
                        {
                            anyUnread = true;
                            break;
                        }
                    }
                    if (anyUnread) unreadIds.Add(c.Id);
                }
                ViewBag.UnreadConversationIds = unreadIds;
            }
            catch { ViewBag.UnreadConversationIds = new List<int>(); }

            return View(conversacion);
        }

        // GET: Conversaciones/SearchUsers?term=
        [HttpGet]
        public IActionResult SearchUsers(string term)
        {
            try
            {
                var usuarios = CRUD<Usuario>.GetAll()
                    .Where(u => u.Nombre_Usuario.Contains(term ?? string.Empty, StringComparison.OrdinalIgnoreCase) || u.Correo_Usuario.Contains(term ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                    .Select(u => new { u.Id, u.Nombre_Usuario, u.Correo_Usuario })
                    .ToList();
                return Json(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: Conversaciones/CreateAjax
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult CreateAjax([FromBody] CreateConversationRequest request)
        {
            try
            {
                // Asegurarse de incluir al usuario actual en los participantes
                var currentUserId = 0;
                var idClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (idClaim != null) int.TryParse(idClaim.Value, out currentUserId);
                var participantesList = request.Participantes != null ? request.Participantes.ToList() : new List<int>();
                if (currentUserId != 0 && !participantesList.Contains(currentUserId)) participantesList.Add(currentUserId);

                var conversacion = new Conversacion
                {
                    Tipo_Conversacion = request.Tipo_Conversacion,
                    Fecha_Creacion_Conversacion = DateTime.Now,
                    Titulo_Conversacion = request.Titulo_Conversacion ?? string.Empty
                };
                // Antes de crear, intentar reutilizar una conversación existente con los mismos participantes
                try
                {
                    var allParticipantes = CRUD<ParticipanteConversacion>.GetAll();
                    var grupos = allParticipantes.GroupBy(p => p.ConversacionId)
                        .Select(g => new { ConversacionId = g.Key, Usuarios = g.Select(x => x.UsuarioId).OrderBy(x => x).ToList() })
                        .ToList();
                    var target = participantesList.OrderBy(x => x).ToList();
                    var existing = grupos.FirstOrDefault(g => g.Usuarios.SequenceEqual(target));
                    if (existing != null)
                    {
                        return Json(new { success = true, id = existing.ConversacionId, note = "reused_existing" });
                    }
                }
                catch { }
                try
                {
                    // Create conversation and get the created object (with Id)
                    var created = CRUD<Conversacion>.Create(conversacion);
                    var createdId = created?.GetType().GetProperty("Id")?.GetValue(created) as int?;
                    int convId = createdId ?? created?.Id ?? conversacion.Id;

                    // Crear participantes usando el id real
                    foreach (var userId in participantesList)
                    {
                        var participante = new ParticipanteConversacion
                        {
                            ConversacionId = convId,
                            UsuarioId = userId,
                            Fecha_Ingreso_Participante = DateTime.Now
                        };
                        CRUD<ParticipanteConversacion>.Create(participante);
                    }

                    return Json(new { success = true, id = convId });
                }
                catch (Exception createEx)
                {
                    // Si la creación falla, intentar buscar una conversación existente que contenga exactamente los participantes
                    try
                    {
                        var allParticipantes = CRUD<ParticipanteConversacion>.GetAll();
                        // agrupar por conversacionId
                        var grupos = allParticipantes.GroupBy(p => p.ConversacionId)
                            .Select(g => new { ConversacionId = g.Key, Usuarios = g.Select(x => x.UsuarioId).OrderBy(x => x).ToList() })
                            .ToList();

                        var target = participantesList.OrderBy(x => x).ToList();
                        var match = grupos.FirstOrDefault(g => g.Usuarios.SequenceEqual(target));
                        if (match != null)
                        {
                            return Json(new { success = true, id = match.ConversacionId, note = "reused_existing" });
                        }
                    }
                    catch { }

                    return Json(new { success = false, error = createEx.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public class CreateConversationRequest
        {
            public bool Tipo_Conversacion { get; set; }
            public string? Titulo_Conversacion { get; set; }
            public int[]? Participantes { get; set; }
        }

        // GET: ConversacionesController/Details/5
        public ActionResult Details(int id)
        {
            var conversacion = CRUD<Conversacion>.GetById(id);
            if(conversacion == null)
            {
                return NotFound();
            }
            // set current user id for the view
            var currentUserId = 0;
            if (User?.Identity?.IsAuthenticated == true)
            {
                var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (idClaim != null) int.TryParse(idClaim.Value, out currentUserId);
            }
            ViewBag.CurrentUserId = currentUserId;

            // participants
            try
            {
                var participantes = CRUD<ParticipanteConversacion>.GetAll()
                    .Where(p => p.ConversacionId == id)
                    .ToList();
                var usuarios = CRUD<Usuario>.GetAll();
                var participantesUsuarios = participantes
                    .Select(p => usuarios.FirstOrDefault(u => u.Id == p.UsuarioId))
                    .Where(u => u != null)
                    .ToList();
                ViewBag.ParticipantesUsuarios = participantesUsuarios;
                // compute display title: if individual chat show the other user's name, else show conversation title
                if (!conversacion.Tipo_Conversacion)
                {
                    var other = participantesUsuarios.FirstOrDefault(u => u.Id != (ViewBag.CurrentUserId as int? ?? 0));
                    ViewBag.TitleName = other != null ? other.Nombre_Usuario : "Chat individual";
                }
                else
                {
                    ViewBag.TitleName = string.IsNullOrWhiteSpace(conversacion.Titulo_Conversacion) ? "Grupo" : conversacion.Titulo_Conversacion;
                }
            }
            catch { }

            return View(conversacion);
        }

        // GET: ConversacionesController/Create
        public ActionResult Create()
        {
            var usuarios = CRUD<Usuario>.GetAll();
            ViewBag.Usuarios = usuarios;
            var currentUserId = 0;
            if (User?.Identity?.IsAuthenticated == true)
            {
                var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (idClaim != null) int.TryParse(idClaim.Value, out currentUserId);
            }
            ViewBag.CurrentUserId = currentUserId;
            return View();
        }

        // POST: ConversacionesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Conversacion conversacion, int[]? participantes)
        {
            try
            {
                var created = CRUD<Conversacion>.Create(conversacion);
                int convId = created?.Id ?? conversacion.Id;

                if (participantes != null)
                {
                    foreach (var userId in participantes)
                    {
                        var participante = new ParticipanteConversacion
                        {
                            ConversacionId = convId,
                            UsuarioId = userId,
                            Fecha_Ingreso_Participante = DateTime.Now
                        };
                        CRUD<ParticipanteConversacion>.Create(participante);
                    }
                }

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
