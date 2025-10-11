using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Modelo
{
    public class Roles
    {
        public int CodigoRol { get; set; }
        public string NombreRol { get; set; }
        public string DescripcionRol { get; set; }
        public int Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
