using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tickets.Modelo
{
    public class Ticket
    {
        public int CodigoTicket { get; set; }
        public string DescripcionTicket { get; set; }
        public int CodigoCliente { get; set; }
        public string TituloTicket { get; set; }
        public int CodigoCategoria { get; set; }
        public int CodigoPrioridad { get; set; }
        public int CodigoDepto { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoEstado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaCierre { get; set; }

    }
}
