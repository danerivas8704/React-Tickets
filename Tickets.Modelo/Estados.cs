using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Modelo
{
    public class Estados
    {
        public int CodigoEstado { get; set; }
        public string NombreEstado { get; set; }
        public string DescripcionEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
