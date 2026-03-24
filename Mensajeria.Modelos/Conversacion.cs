using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Modelos
{
    public class Conversacion
    {
        [Key] public int Id { get; set; }

        public string Tipo_Conversacion { get; set; }
        public DateTime Fecha_Creacion_Conversacion { get; set; }
        public string Titulo_Conversacion { get; set; }

        //Se lo inicializa para evitar errores de referencia nula
        List<ParticipanteConversacion>? Participantes { get; set; } = new List<ParticipanteConversacion>(); 
        List<Mensaje>? Mensajes { get; set; } = new List<Mensaje>();
    }
}
