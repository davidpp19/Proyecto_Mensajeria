using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Modelos
{
    public class Mensaje
    {
        [Key] public int Id { get; set; }
        public string Contenido_Mensaje { get; set; }
        public TimeOnly Hora_Envio_Mensaje { get; set; }

        [ForeignKey("ConversacionId")] public int ConversacionId { get; set; }
        [ForeignKey("RemitenteId")] public int RemitenteId { get; set; }

        //Objetos de navegación
        public Conversacion? Conversacion { get; set; }
        public Usuario? Remitente { get; set; }

        List<MensajeLectura>? MensajesLectura { get; set; } = new List<MensajeLectura>(); 
    }
}
