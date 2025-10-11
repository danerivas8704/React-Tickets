using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Modelo
{
    public class Prioridades
    {
        public int CodigoPrioridad { get; set; }
        public string NombrePrioridad { get; set; }
        public string DescripcionPrioridad { get; set; }
        public int NivelPrioridad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
