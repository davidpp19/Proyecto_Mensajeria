using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Modelos
{
    public class Usuario
    {
        [Key] public int Id { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Correo_Usuario { get; set; }
        public string Contrasena_Usuario { get; set; } 

        public DateTime Fecha_Creacion_Usuario { get; set; }
        public Boolean Estado_Usuario { get; set; }

        [ForeignKey("RolId")] public int RolId { get; set; }

        //Objeto de navegación
        public Rol? Rol { get; set; }
        List<Mensaje>? Mensajes { get; set; } = new List<Mensaje>();

        List<MensajeLectura>? MensajesLectura { get; set; } = new List<MensajeLectura>();
        List<ParticipanteConversacion>? ParticipantesConversacions { get; set; } = new List<ParticipanteConversacion>();
    }
}
