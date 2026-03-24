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
    public class ConversacionesController : ControllerBase
    {
        private readonly MensajeriaAPIContext _context;

        public ConversacionesController(MensajeriaAPIContext context)
        {
            _context = context;
        }

        // GET: api/Conversaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversacion>>> GetConversacion()
        {
            return await _context.Conversacion.ToListAsync();
        }

        // GET: api/Conversaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Conversacion>> GetConversacion(int id)
        {
            var conversacion = await _context.Conversacion.FindAsync(id);

            if (conversacion == null)
            {
                return NotFound();
            }

            return conversacion;
        }

        // PUT: api/Conversaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConversacion(int id, Conversacion conversacion)
        {
            if (id != conversacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(conversacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversacionExists(id))
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

        // POST: api/Conversaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Conversacion>> PostConversacion(Conversacion conversacion)
        {
            _context.Conversacion.Add(conversacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConversacion", new { id = conversacion.Id }, conversacion);
        }

        // DELETE: api/Conversaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConversacion(int id)
        {
            var conversacion = await _context.Conversacion.FindAsync(id);
            if (conversacion == null)
            {
                return NotFound();
            }

            _context.Conversacion.Remove(conversacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConversacionExists(int id)
        {
            return _context.Conversacion.Any(e => e.Id == id);
        }
    }
}
