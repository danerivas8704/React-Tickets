using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tickets.Modelo
{
    public class Clientes
    {
        
        public int CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string CorreoCliente { get; set; }
        public string DireccionCliente { get; set; }
        public int CodigoEstado { get; set; }
    }
}
