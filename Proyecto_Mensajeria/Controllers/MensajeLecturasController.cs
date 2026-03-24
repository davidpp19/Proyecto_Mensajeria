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
    public class MensajeLecturasController : ControllerBase
    {
        private readonly MensajeriaAPIContext _context;

        public MensajeLecturasController(MensajeriaAPIContext context)
        {
            _context = context;
        }

        // GET: api/MensajeLecturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MensajeLectura>>> GetMensajeLectura()
        {
            // Incluimos las entidades relacionadas: Mensaje, Usuario y EstadoMensaje
            // Usamos expresiones lambda para incluir las relaciones y evitar problemas de serialización

            var mensajeLectura = await _context.MensajeLectura.Include(a => a.Mensaje).Include(a => a.Usuario).Include(a => a.EstadoMensaje).ToListAsync();
            return mensajeLectura;
        }

        // GET: api/MensajeLecturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MensajeLectura>> GetMensajeLectura(int id)
        {
            var mensajeLectura = await _context.MensajeLectura.Include(a => a.Mensaje).Include(a => a.Usuario).Include(a => a.EstadoMensaje).FirstOrDefaultAsync(a => a.MensajeId == id);

            if (mensajeLectura == null)
            {
                return NotFound();
            }

            return mensajeLectura;
        }

        // PUT: api/MensajeLecturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensajeLectura(int id, MensajeLectura mensajeLectura)
        {
            if (id != mensajeLectura.MensajeId)
            {
                return BadRequest();
            }

            _context.Entry(mensajeLectura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajeLecturaExists(id))
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

        // POST: api/MensajeLecturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MensajeLectura>> PostMensajeLectura(MensajeLectura mensajeLectura)
        {
            _context.MensajeLectura.Add(mensajeLectura);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MensajeLecturaExists(mensajeLectura.MensajeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMensajeLectura", new { id = mensajeLectura.MensajeId }, mensajeLectura);
        }

        // DELETE: api/MensajeLecturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMensajeLectura(int id)
        {
            var mensajeLectura = await _context.MensajeLectura.FindAsync(id);
            if (mensajeLectura == null)
            {
                return NotFound();
            }

            _context.MensajeLectura.Remove(mensajeLectura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MensajeLecturaExists(int id)
        {
            return _context.MensajeLectura.Any(e => e.MensajeId == id);
        }
    }
}
