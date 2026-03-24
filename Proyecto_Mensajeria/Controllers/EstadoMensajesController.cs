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
    public class EstadoMensajesController : ControllerBase
    {
        private readonly MensajeriaAPIContext _context;

        public EstadoMensajesController(MensajeriaAPIContext context)
        {
            _context = context;
        }

        // GET: api/EstadosMensajes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoMensaje>>> GetEstadoMensaje()
        {
            return await _context.EstadoMensaje.ToListAsync();
        }

        // GET: api/EstadosMensajes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoMensaje>> GetEstadoMensaje(int id)
        {
            var estadoMensaje = await _context.EstadoMensaje.FindAsync(id);

            if (estadoMensaje == null)
            {
                return NotFound();
            }

            return estadoMensaje;
        }

        // PUT: api/EstadosMensajes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoMensaje(int id, EstadoMensaje estadoMensaje)
        {
            if (id != estadoMensaje.Id)
            {
                return BadRequest();
            }

            _context.Entry(estadoMensaje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoMensajeExists(id))
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

        // POST: api/EstadosMensajes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstadoMensaje>> PostEstadoMensaje(EstadoMensaje estadoMensaje)
        {
            _context.EstadoMensaje.Add(estadoMensaje);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoMensaje", new { id = estadoMensaje.Id }, estadoMensaje);
        }

        // DELETE: api/EstadosMensajes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoMensaje(int id)
        {
            var estadoMensaje = await _context.EstadoMensaje.FindAsync(id);
            if (estadoMensaje == null)
            {
                return NotFound();
            }

            _context.EstadoMensaje.Remove(estadoMensaje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoMensajeExists(int id)
        {
            return _context.EstadoMensaje.Any(e => e.Id == id);
        }
    }
}
