using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using Tickets.Modelo;

namespace Tickets.Datos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EstadosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ObtenerCodigo/{id:int}")]
        public async Task<List<Estados>> ObtenerCodigo(int id)
        {
            string consulta = @"Select * from adm_cat_estados where codigoestado=@id";
            List<Estados> lstEstados = new List<Estados>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();

                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@id", id);
                        using (var drEstados = await mscom.ExecuteReaderAsync())
                        {
                            while (drEstados.Read())
                            {
                                lstEstados.Add(new Estados
                                {
                                    CodigoEstado = Convert.ToInt32(drEstados["codigoestado"]),
                                    NombreEstado = drEstados["nombreestado"].ToString(),
                                    DescripcionEstado = drEstados["descripcionestado"].ToString(),
                                    FechaCreacion = Convert.ToDateTime(drEstados["fechacreacion"])
                                });
                            }
                            drEstados.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstEstados;
        }

        [HttpGet]
        [Route("Obtener")]
        public async Task<List<Estados>> ObtenerTodos()
        {
            string consulta = @"Select * from adm_cat_estados";
            List<Estados> lstEstados = new List<Estados>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        using (var drEstados = await mscom.ExecuteReaderAsync())
                        {
                            while (drEstados.Read())
                            {
                                lstEstados.Add(new Estados
                                {
                                    CodigoEstado = Convert.ToInt32(drEstados["codigoestado"]),
                                    NombreEstado = drEstados["nombreestado"].ToString(),
                                    DescripcionEstado = drEstados["descripcionestado"].ToString(),
                                    FechaCreacion = Convert.ToDateTime(drEstados["fechacreacion"]),
                                    FechaModificacion = Convert.ToDateTime(drEstados["fechamodificacion"])                                  
                                });
                            }
                            drEstados.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstEstados;
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<bool> Crear(Estados estados)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Insert into adm_cat_estados(nombreestado,
                                descripcionestado,fechacreacion, fechamodificacion)
                                values
                                (@nombreestado,@descripcionestado,
                                @fechacreacion,@fechamodificacion)
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@nombreestado", estados.NombreEstado);
                        mscom.Parameters.AddWithValue("@descripcionestado", estados.DescripcionEstado);
                        mscom.Parameters.AddWithValue("@fechacreacion", DateTime.Now.ToString("yyyy-MM-dd"));
                        mscom.Parameters.AddWithValue("@fechamodificacion", estados.FechaModificacion);                       
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

        [HttpPut]
        [Route("Editar")]
        public async Task<bool> Editar(Estados estados)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Update adm_cat_estados SET
                                nombreestado = @nombreestado,
                                descripcionestado =@descripcionestado,
                                fechacreacion = @fechacreacion,
                                fechamodificacion =@fechamodificacion                                                              
                                WHERE
                                codigoestado = @codigoestado;
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@codigoestado", estados.CodigoEstado);
                        mscom.Parameters.AddWithValue("@nombreestado", estados.NombreEstado);
                        mscom.Parameters.AddWithValue("@descripcionestado", estados.DescripcionEstado);
                        mscom.Parameters.AddWithValue("@fechacreacion", estados.FechaCreacion);
                        mscom.Parameters.AddWithValue("@fechamodificacion", estados.FechaModificacion);                        
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

