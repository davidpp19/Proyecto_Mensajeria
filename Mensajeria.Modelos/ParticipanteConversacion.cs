using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Modelos
{
    [PrimaryKey(nameof(ConversacionId), nameof(UsuarioId))] //Usamos la anotación [PrimaryKey] para indicar que la clave primaria está compuesta por ConversacionId y UsuarioId
    public class ParticipanteConversacion
    {
        public int ConversacionId { get; set; }
        public int UsuarioId { get; set; } 
        public DateTime Fecha_Ingreso_Participante { get; set; }

        [ForeignKey("ConversacionId")]
        public Conversacion? Conversacion { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

    }
}
