using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Modelos
{
    public class ParticipanteConversacion
    {
        [Key] public int ConversacionId { get; set; }
        [Key] public int UsuarioId { get; set; } 
        public DateTime Fecha_Ingreso_Participante { get; set; }

        [ForeignKey("ConversacionId")]
        public Conversacion? Conversacion { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

    }
}
