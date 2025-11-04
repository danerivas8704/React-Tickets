using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using Tickets.Modelo;

namespace Tickets.Datos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ClientesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ClientesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ObtenerC")]
        public async Task<List<Catalogo>> ObtenerTodosCombo()
        {
            string consulta = @"Select codigocliente,nombrecliente from adm_cat_clientes";
            List<Catalogo> lstClientes = new List<Catalogo>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        using (var drClientes = await mscom.ExecuteReaderAsync())
                        {
                            while (drClientes.Read())
                            {
                                lstClientes.Add(new Catalogo
                                {
                                    Id = Convert.ToInt32(drClientes["codigocliente"]),
                                    Nombre = drClientes["nombrecliente"].ToString()

                                });
                            }
                            drClientes.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstClientes;
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            string consulta = @"DELETE FROM adm_cat_clientes WHERE codigocliente = @id";
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                        if (filasAfectadas > 0)
                        {
                            return Ok(new
                            {
                                success = true,
                                message = "Cliente eliminado correctamente."
                            });
                        }
                        else
                        {
                            return NotFound(new
                            {
                                success = false,
                                message = "No se encontró el cliente."
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Ocurrió un error al eliminar el cliente.",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("ObtenerCodigo/{id:int}")]
        public async Task<List<Clientes>> ObtenerCodigo(int id)
        {
            string consulta = @"Select * from adm_cat_clientes where codigocliente=@id";
            List<Clientes> lstClientes = new List<Clientes>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@id", id);
                        using (var drClientes = await mscom.ExecuteReaderAsync())
                        {
                            while (drClientes.Read())
                            {
                                lstClientes.Add(new Clientes
                                {
                                    CodigoCliente = Convert.ToInt32(drClientes["codigocliente"]),
                                    NombreCliente = drClientes["nombrecliente"].ToString(),
                                    ApellidoCliente = drClientes["apellidocliente"].ToString(),
                                    CorreoCliente = drClientes["correo"].ToString(),
                                    DireccionCliente = drClientes["direccion"].ToString(),
                                    CodigoEstado = Convert.ToInt32(drClientes["codigoestado"])
                                });
                            }
                            drClientes.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstClientes;
        }

        [HttpGet]
        [Route("Obtener")]       
        public async Task<List<Clientes>> ObtenerTodos()
        {
            string consulta = @"Select * from adm_cat_clientes";
            List<Clientes> lstClientes = new List<Clientes>();            
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");
            
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {                        
                        using (var drClientes = await mscom.ExecuteReaderAsync())
                        {
                            while (drClientes.Read())
                            {
                                lstClientes.Add(new Clientes
                                {
                                    CodigoCliente = Convert.ToInt32(drClientes["codigocliente"]),
                                    NombreCliente = drClientes["nombrecliente"].ToString(),
                                    ApellidoCliente = drClientes["apellidocliente"].ToString(),
                                    CorreoCliente = drClientes["correo"].ToString(),
                                    DireccionCliente = drClientes["direccion"].ToString(),
                                    CodigoEstado = Convert.ToInt32(drClientes["codigoestado"])
                                });   
                            }                            
                            drClientes.Close();
                            con.Close();
                        }
                    }                                        
                }                
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstClientes;
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<bool> Crear(Clientes clientes)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");
            
            string consulta = @"Insert into adm_cat_clientes(nombrecliente,apellidocliente,
                                correo, direccion, codigoestado)
                                values
                                (@nombrecliente,@apellidocliente,@correo,
                                @direccion,@codigoestado)
                              ";            
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@nombrecliente",clientes.NombreCliente);
                        mscom.Parameters.AddWithValue("@apellidocliente",clientes.ApellidoCliente);
                        mscom.Parameters.AddWithValue("@correo",clientes.CorreoCliente);
                        mscom.Parameters.AddWithValue("@direccion",clientes.DireccionCliente);
                        mscom.Parameters.AddWithValue("@codigoestado",clientes.CodigoEstado);
                        mscom.CommandType = CommandType.Text;
                        blnRespuesta = await mscom.ExecuteNonQueryAsync() > 0 ? true:false;                        
                        con.Close();                        
                    }
                }
            }
            catch (Exception ex)
            {
                blnRespuesta = false;
            }
            return blnRespuesta;
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<bool> Editar(Clientes clientes)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Update adm_cat_clientes SET
                                nombrecliente = @nombrecliente,
                                apellidocliente =@apellidocliente,
                                correo = @correo,
                                direccion =@direccion,
                                codigoestado=@codigoestado                                
                                WHERE
                                codigocliente = @codigocliente;
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@codigocliente", clientes.CodigoCliente);
                        mscom.Parameters.AddWithValue("@nombrecliente", clientes.NombreCliente);
                        mscom.Parameters.AddWithValue("@apellidocliente", clientes.ApellidoCliente);
                        mscom.Parameters.AddWithValue("@correo", clientes.CorreoCliente);
                        mscom.Parameters.AddWithValue("@direccion", clientes.DireccionCliente);
                        mscom.Parameters.AddWithValue("@codigoestado", clientes.CodigoEstado);
                        mscom.CommandType = CommandType.Text;
                        blnRespuesta = await mscom.ExecuteNonQueryAsync() > 0 ? true : false;
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                blnRespuesta = false;
            }
            return blnRespuesta;
        }
    }
}