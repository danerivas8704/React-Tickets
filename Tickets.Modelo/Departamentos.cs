using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Modelo
{
    public class Departamentos
    {
        public int CodigoDepto { get; set; }
        public string NombreDepto { get; set; }
        public string DescripcionDepto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int CodigoEstado { get; set; }

    }
}
