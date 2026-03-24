using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Modelos
{
    [PrimaryKey(nameof(MensajeId), nameof(UsuarioId))] //Usamos la anotación [PrimaryKey] para indicar que la clave primaria está compuesta por MensajeId y UsuarioId
    public class MensajeLectura
    {
        public int MensajeId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha_Lectura { get; set; }

        [ForeignKey("MensajeId")]
        public Mensaje? Mensaje { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [ForeignKey("EstadoMensajeId")] public int EstadoMensajeId { get; set; }
        public EstadoMensaje? EstadoMensaje { get; set; }
    }
}
