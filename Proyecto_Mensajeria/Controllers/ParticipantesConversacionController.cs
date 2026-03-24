using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mensajeria.API.Data;
using Mensajeria.Modelos;

namespace Mensajeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantesConversacionController : ControllerBase
    {
        private readonly MensajeriaAPIContext _context;

        public ParticipantesConversacionController(MensajeriaAPIContext context)
        {
            _context = context;
        }

        // GET: api/ParticipantesConversacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipanteConversacion>>> GetParticipanteConversacion()
        {
            var participante = await _context.ParticipanteConversacion.Include(p => p.Conversacion).Include(p => p.Usuario).ToListAsync();
            return participante;
        }

        // GET: api/ParticipantesConversacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipanteConversacion>> GetParticipanteConversacion(int id)
        {
            var participanteConversacion = await _context.ParticipanteConversacion.Include(p => p.Conversacion).Include(p => p.Usuario).FirstOrDefaultAsync(p => p.ConversacionId == id);

            if (participanteConversacion == null)
            {
                return NotFound();
            }

            return participanteConversacion;
        }

        // PUT: api/ParticipantesConversacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipanteConversacion(int id, ParticipanteConversacion participanteConversacion)
        {
            if (id != participanteConversacion.ConversacionId)
            {
                return BadRequest();
            }

            _context.Entry(participanteConversacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipanteConversacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ParticipantesConversacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParticipanteConversacion>> PostParticipanteConversacion(ParticipanteConversacion participanteConversacion)
        {
            _context.ParticipanteConversacion.Add(participanteConversacion);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ParticipanteConversacionExists(participanteConversacion.ConversacionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetParticipanteConversacion", new { id = participanteConversacion.ConversacionId }, participanteConversacion);
        }

        // DELETE: api/ParticipantesConversacion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipanteConversacion(int id)
        {
            var participanteConversacion = await _context.ParticipanteConversacion.FindAsync(id);
            if (participanteConversacion == null)
            {
                return NotFound();
            }

            _context.ParticipanteConversacion.Remove(participanteConversacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticipanteConversacionExists(int id)
        {
            return _context.ParticipanteConversacion.Any(e => e.ConversacionId == id);
        }
    }
}
