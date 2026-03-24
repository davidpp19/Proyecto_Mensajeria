using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Modelos
{
    public class EstadoMensaje
    {
        [Key] public int Id { get; set; }

        public string Descripcion_Estado { get; set; }
        //Se lo inicializa para evitar errores de referencia nula
        List<MensajeLectura>? MensajesLectura { get; set; } = new List<MensajeLectura>();
    }
}
