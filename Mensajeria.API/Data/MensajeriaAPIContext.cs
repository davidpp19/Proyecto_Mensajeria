using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mensajeria.Modelos;

namespace Mensajeria.API.Data
{
    public class MensajeriaAPIContext : DbContext
    {
        public MensajeriaAPIContext (DbContextOptions<MensajeriaAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Mensajeria.Modelos.EstadoMensaje> EstadoMensaje { get; set; } = default!;
        public DbSet<Mensajeria.Modelos.Rol> Rol { get; set; } = default!;
        public DbSet<Mensajeria.Modelos.Conversacion> Conversacion { get; set; } = default!;
        public DbSet<Mensajeria.Modelos.Usuario> Usuario { get; set; } = default!;
        public DbSet<Mensajeria.Modelos.Mensaje> Mensaje { get; set; } = default!;
        public DbSet<Mensajeria.Modelos.ParticipanteConversacion> ParticipanteConversacion { get; set; } = default!;
        public DbSet<Mensajeria.Modelos.MensajeLectura> MensajeLectura { get; set; } = default!;
    }
}
