using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Modelo
{
    public class Usuarios
    {
        public int CodigoUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public int CodigoDepto { get; set; }
        public string Password { get; set; }
        public int CodigoEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime UltimoLogin { get; set; }

    }
}
