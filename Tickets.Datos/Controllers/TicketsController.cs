using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using Tickets.Modelo;

namespace Tickets.Datos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TicketsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ObtenerDashboard")]
        public async Task<List<Dashboard>> ObtenerDashboard()
        {
            List<Dashboard> lstDashboard = new List<Dashboard>();
            string consulta = @"SELECT
                                SUM(CASE WHEN e.nombreEstado = 'Activo' THEN 1 ELSE 0 END) AS Abierto,
                                SUM(CASE WHEN e.nombreEstado = 'En progreso' THEN 1 ELSE 0 END) AS EnProgreso,
                                SUM(CASE WHEN e.nombreEstado = 'Resuelto' THEN 1 ELSE 0 END) AS Resuelto,
                                SUM(CASE WHEN e.nombreEstado = 'Cerrado' THEN 1 ELSE 0 END) AS Cerrado
                                FROM adm_tickets t
                                INNER JOIN adm_cat_estados e ON t.codigoEstado = e.codigoEstado;";
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    using (var dr = await mscom.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lstDashboard.Add(new Dashboard
                            {
                                Abierto = Convert.ToInt32(dr["abierto"]),
                                EnProgreso = Convert.ToInt32(dr["enprogreso"]),
                                Resuelto = Convert.ToInt32(dr["resuelto"]),
                                Cerrado = Convert.ToInt32(dr["cerrado"]),

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener los tickets", ex);
            }

            return lstDashboard;
        }

        // 🔹 Obtener todos los tickets
        [HttpGet]
        [Route("Obtener")]
        public async Task<List<Ticket>> ObtenerTodos()
        {
            List<Ticket> lstTickets = new List<Ticket>();
            string consulta = @"SELECT * FROM adm_tickets";
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    using (var dr = await mscom.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lstTickets.Add(new Ticket
                            {
                                CodigoTicket = Convert.ToInt32(dr["codigoticket"]),
                                DescripcionTicket = dr["descripcionticket"].ToString(),
                                CodigoCliente = Convert.ToInt32(dr["codigocliente"]),
                                TituloTicket = dr["tituloticket"].ToString(),
                                CodigoCategoria = Convert.ToInt32(dr["codigocategoria"]),
                                CodigoPrioridad = Convert.ToInt32(dr["codigoprioridad"]),
                                CodigoDepto = Convert.ToInt32(dr["codigodepto"]),
                                CodigoUsuario = Convert.ToInt32(dr["codigousuario"]),
                                CodigoEstado = Convert.ToInt32(dr["codigoestado"]),
                                UsuarioCreacion = dr["usuariocreacion"].ToString(),
                                FechaCreacion = Convert.ToDateTime(dr["fechacreacion"]),
                                FechaModificacion = Convert.ToDateTime(dr["fechamodificacion"]),
                                FechaCierre = Convert.ToDateTime(dr["fechacierre"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener los tickets", ex);
            }

            return lstTickets;
        }

        // 🔹 Obtener un ticket por código
        [HttpGet]
        [Route("ObtenerCodigo/{id:int}")]
        public async Task<List<Ticket>> ObtenerCodigo(int id)
        {
            List<Ticket> lstTickets = new List<Ticket>();
            string consulta = "SELECT * FROM adm_tickets WHERE codigoticket = @id";
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@id", id);
                        using (var dr = await mscom.ExecuteReaderAsync())
                        {
                            while (await dr.ReadAsync())
                            {
                                lstTickets.Add(new Ticket
                                {
                                    CodigoTicket = Convert.ToInt32(dr["codigoticket"]),
                                    DescripcionTicket = dr["descripcionticket"].ToString(),
                                    CodigoCliente = Convert.ToInt32(dr["codigocliente"]),
                                    TituloTicket = dr["tituloticket"].ToString(),
                                    CodigoCategoria = Convert.ToInt32(dr["codigocategoria"]),
                                    CodigoPrioridad = Convert.ToInt32(dr["codigoprioridad"]),
                                    CodigoDepto = Convert.ToInt32(dr["codigodepto"]),
                                    CodigoUsuario = Convert.ToInt32(dr["codigousuario"]),
                                    CodigoEstado = Convert.ToInt32(dr["codigoestado"]),
                                    UsuarioCreacion = dr["usuariocreacion"].ToString(),
                                    FechaCreacion = Convert.ToDateTime(dr["fechacreacion"]),
                                    FechaModificacion = Convert.ToDateTime(dr["fechamodificacion"]),
                                    FechaCierre = Convert.ToDateTime(dr["fechacierre"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener el ticket por código", ex);
            }

            return lstTickets;
        }

        // 🔹 Crear un nuevo ticket
        [HttpPost]
        [Route("Crear")]
        public async Task<bool> Crear(Ticket ticket)
        {
            bool resultado = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"INSERT INTO adm_tickets
                                (descripcionticket, codigocliente, tituloticket, codigocategoria, 
                                codigoprioridad, codigodepto, codigousuario, codigoestado, 
                                usuariocreacion, fechacreacion, fechamodificacion, fechacierre)
                                VALUES
                                (@descripcionticket, @codigocliente, @tituloticket, @codigocategoria,
                                @codigoprioridad, @codigodepto, @codigousuario, @codigoestado,
                                @usuariocreacion, @fechacreacion, @fechamodificacion, @fechacierre)";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                    {
                        cmd.Parameters.AddWithValue("@descripcionticket", ticket.DescripcionTicket);
                        cmd.Parameters.AddWithValue("@codigocliente", ticket.CodigoCliente);
                        cmd.Parameters.AddWithValue("@tituloticket", ticket.TituloTicket);
                        cmd.Parameters.AddWithValue("@codigocategoria", ticket.CodigoCategoria);
                        cmd.Parameters.AddWithValue("@codigoprioridad", ticket.CodigoPrioridad);
                        cmd.Parameters.AddWithValue("@codigodepto", ticket.CodigoDepto);
                        cmd.Parameters.AddWithValue("@codigousuario", ticket.CodigoUsuario);
                        cmd.Parameters.AddWithValue("@codigoestado", ticket.CodigoEstado);
                        cmd.Parameters.AddWithValue("@usuariocreacion", ticket.UsuarioCreacion);
                        cmd.Parameters.AddWithValue("@fechacreacion", ticket.FechaCreacion);
                        cmd.Parameters.AddWithValue("@fechamodificacion", ticket.FechaModificacion);
                        cmd.Parameters.AddWithValue("@fechacierre", ticket.FechaCierre);

                        resultado = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                throw new ApplicationException("Error al crear el ticket", ex);
            }

            return resultado;
        }

            [HttpGet]
            [Route("Reporte")]
            public async Task<List<Ticket>> Reporte()
            {
                List<Ticket> lstTickets = new List<Ticket>();

                string sqlDs = _configuration.GetConnectionString("CadenaMysql");
                string consulta = @"SELECT 
                                    t.codigoticket,
                                    t.tituloticket,
                                    t.descripcionticket,
                                    c.nombrecliente,
                                    cat.nombrecategoria,
                                    p.nombreprioridad,
                                    d.nombredepto,
                                    u.nombreusuario,
                                    e.nombreestado,
                                    t.fechacreacion,
                                    t.fechamodificacion,
                                    t.fechacierre
                                FROM adm_tickets t
                                INNER JOIN adm_cat_clientes c ON t.codigocliente = c.codigocliente
                                INNER JOIN adm_cat_categoria cat ON t.codigocategoria = cat.codigocategoria
                                INNER JOIN adm_cat_prioridades p ON t.codigoprioridad = p.codigoprioridad
                                INNER JOIN adm_cat_depto d ON t.codigodepto = d.codigodepto
                                INNER JOIN adm_cat_usuarios u ON t.codigousuario = u.codigousuario
                                INNER JOIN adm_cat_estados e ON t.codigoestado = e.codigoestado";

                try
                {
                    using (MySqlConnection con = new MySqlConnection(sqlDs))
                    {
                        await con.OpenAsync();
                        using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                        {
                            using (var dr = await mscom.ExecuteReaderAsync())
                            {
                                while (await dr.ReadAsync())
                                {
                                    lstTickets.Add(new Ticket
                                    {
                                        CodigoTicket = Convert.ToInt32(dr["codigoticket"]),
                                        TituloTicket = dr["tituloticket"].ToString(),
                                        DescripcionTicket = dr["descripcionticket"].ToString(),
                                        CodigoCliente = 0, // no se necesita mostrar, ya se trae el nombre
                                        CodigoCategoria = 0,
                                        CodigoPrioridad = 0,
                                        CodigoDepto = 0,
                                        CodigoUsuario = 0,
                                        CodigoEstado = 0,
                                        UsuarioCreacion = dr["nombreusuario"].ToString(),
                                        FechaCreacion = Convert.ToDateTime(dr["fechacreacion"])
                                        
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error generando el reporte de tickets.", ex);
                }

                return lstTickets;
            }
        
    


    // 🔹 Editar un ticket existente
        [HttpPut]
        [Route("Editar")]
        public async Task<bool> Editar(Ticket ticket)
        {
            bool resultado = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"UPDATE adm_tickets SET
                                descripcionticket=@descripcionticket,
                                codigocliente=@codigocliente,
                                tituloticket=@tituloticket,
                                codigocategoria=@codigocategoria,
                                codigoprioridad=@codigoprioridad,
                                codigodepto=@codigodepto,
                                codigousuario=@codigousuario,
                                codigoestado=@codigoestado,
                                usuariocreacion=@usuariocreacion,
                                fechacreacion=@fechacreacion,
                                fechamodificacion=@fechamodificacion,
                                fechacierre=@fechacierre
                                WHERE codigoticket=@codigoticket";

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                    {
                        cmd.Parameters.AddWithValue("@codigoticket", ticket.CodigoTicket);
                        cmd.Parameters.AddWithValue("@descripcionticket", ticket.DescripcionTicket);
                        cmd.Parameters.AddWithValue("@codigocliente", ticket.CodigoCliente);
                        cmd.Parameters.AddWithValue("@tituloticket", ticket.TituloTicket);
                        cmd.Parameters.AddWithValue("@codigocategoria", ticket.CodigoCategoria);
                        cmd.Parameters.AddWithValue("@codigoprioridad", ticket.CodigoPrioridad);
                        cmd.Parameters.AddWithValue("@codigodepto", ticket.CodigoDepto);
                        cmd.Parameters.AddWithValue("@codigousuario", ticket.CodigoUsuario);
                        cmd.Parameters.AddWithValue("@codigoestado", ticket.CodigoEstado);
                        cmd.Parameters.AddWithValue("@usuariocreacion", ticket.UsuarioCreacion);
                        cmd.Parameters.AddWithValue("@fechacreacion", ticket.FechaCreacion);
                        cmd.Parameters.AddWithValue("@fechamodificacion", ticket.FechaModificacion);
                        cmd.Parameters.AddWithValue("@fechacierre", ticket.FechaCierre);

                        resultado = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                throw new ApplicationException("Error al editar el ticket", ex);
            }

            return resultado;
        }
    }
}
