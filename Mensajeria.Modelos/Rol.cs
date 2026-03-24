using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Modelos
{
    public class Rol
    {
        [Key] public int Id { get; set; }
        public string Nombre_Rol { get; set; }

        List<Usuario>? Usuarios { get; set; } = new List<Usuario>(); //Se lo inicializa para evitar errores de referencia nula
    }
}
