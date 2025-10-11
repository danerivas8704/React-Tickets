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
    public class PrioridadesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PrioridadesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Obtener")]
        public async Task<List<Prioridades>> ObtenerTodos()
        {
            string consulta = @"Select * from adm_cat_prioridades";
            List<Prioridades> lstPrioridades = new List<Prioridades>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        using (var drPrioridades = await mscom.ExecuteReaderAsync())
                        {
                            while (drPrioridades.Read())
                            {
                                lstPrioridades.Add(new Prioridades
                                {
                                    CodigoPrioridad = Convert.ToInt32(drPrioridades["codigoprioridad"]),
                                    NombrePrioridad = drPrioridades["nombrerol"].ToString(),
                                    DescripcionPrioridad = drPrioridades["descripcionrol"].ToString(),
                                    NivelPrioridad = Convert.ToInt32(drPrioridades["estado"]),
                                    FechaCreacion = Convert.ToDateTime(drPrioridades["fechacreacion"]),
                                    FechaModificacion = Convert.ToDateTime(drPrioridades["fechamodificacion"])
                                });
                            }
                            drPrioridades.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstPrioridades;
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<bool> Crear(Prioridades prioridad)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Insert into adm_cat_prioridades(nombreprioridad,descripcionprioridad,
                                nivelprioridad,fechacreacion, fechamodificacion)
                                values
                                (@nombreprioridad,@descripcionprioridad,@nivelprioridad,
                                @fechacreacion,@fechamodificacion)
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@nombreprioridad", prioridad.NombrePrioridad);
                        mscom.Parameters.AddWithValue("@descripcionprioridad", prioridad.DescripcionPrioridad);
                        mscom.Parameters.AddWithValue("@nivelprioridad", prioridad.NivelPrioridad);
                        mscom.Parameters.AddWithValue("@fechacreacion", DateTime.Now.ToString("yyyy-MM-dd"));
                        mscom.Parameters.AddWithValue("@fechamodificacion", prioridad.FechaModificacion);
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
        public async Task<bool> Editar(Prioridades prioridad)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Update adm_cat_prioridades SET
                                nombreprioridad = @nombreprioridad,
                                descripcionprioridad =@descripcionprioridad,
                                nivelprioridad = @nivelprioridad,
                                fechacreacion =@fechacreacion,
                                fechamodificacion=@fechamodificacion                                
                                WHERE
                                codigoprioridad = @codigoprioridad;
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@codigoprioridad", prioridad.CodigoPrioridad);
                        mscom.Parameters.AddWithValue("@nombreprioridad", prioridad.NombrePrioridad);
                        mscom.Parameters.AddWithValue("@descripcionprioridad", prioridad.DescripcionPrioridad);
                        mscom.Parameters.AddWithValue("@nivelprioridad", prioridad.NivelPrioridad);
                        mscom.Parameters.AddWithValue("@fechacreacion", prioridad.FechaCreacion);
                        mscom.Parameters.AddWithValue("@fechamodificacion", prioridad.FechaModificacion);
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
