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
    public class MensajesController : ControllerBase
    {
        private readonly MensajeriaAPIContext _context;

        public MensajesController(MensajeriaAPIContext context)
        {
            _context = context;
        }

        // GET: api/Mensajes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mensaje>>> GetMensaje()
        {
            var mensaje = await _context.Mensaje.Include(m => m.Conversacion).Include(m => m.Remitente).ToListAsync();
            return mensaje;
        }

        // GET: api/Mensajes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mensaje>> GetMensaje(int id)
        {
            var mensaje = await _context.Mensaje.Include(m => m.Conversacion).Include(m => m.Remitente).FirstOrDefaultAsync(m => m.Id == id);

            if (mensaje == null)
            {
                return NotFound();
            }

            return mensaje;
        }

        // PUT: api/Mensajes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensaje(int id, Mensaje mensaje)
        {
            if (id != mensaje.Id)
            {
                return BadRequest();
            }

            _context.Entry(mensaje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajeExists(id))
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

        // POST: api/Mensajes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mensaje>> PostMensaje(Mensaje mensaje)
        {
            _context.Mensaje.Add(mensaje);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMensaje", new { id = mensaje.Id }, mensaje);
        }

        // DELETE: api/Mensajes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMensaje(int id)
        {
            var mensaje = await _context.Mensaje.FindAsync(id);
            if (mensaje == null)
            {
                return NotFound();
            }

            _context.Mensaje.Remove(mensaje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MensajeExists(int id)
        {
            return _context.Mensaje.Any(e => e.Id == id);
        }
    }
}
