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
    public class RolesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RolesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Obtener")]
        public async Task<List<Roles>> ObtenerTodos()
        {
            string consulta = @"Select * from adm_cat_roles";
            List<Roles> lstRoles = new List<Roles>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        using (var drRoles = await mscom.ExecuteReaderAsync())
                        {
                            while (drRoles.Read())
                            {
                                lstRoles.Add(new Roles
                                {
                                    CodigoRol = Convert.ToInt32(drRoles["codigorol"]),
                                    NombreRol = drRoles["nombrerol"].ToString(),
                                    DescripcionRol = drRoles["descripcionrol"].ToString(),
                                    Estado = Convert.ToInt32(drRoles["estado"]),
                                    FechaCreacion = Convert.ToDateTime(drRoles["fechacreacion"]),
                                    FechaModificacion = Convert.ToDateTime(drRoles["fechamodificacion"])
                                });
                            }
                            drRoles.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstRoles;
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<bool> Crear(Roles rol)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Insert into adm_cat_roles(nombrerol,descripcionrol,
                                estado,fechacreacion, fechamodificacion)
                                values
                                (@nombrerol,@descripcionrol,@estado,
                                @fechacreacion,@fechamodificacion)
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@nombredepto", rol.NombreRol);
                        mscom.Parameters.AddWithValue("@descripciondepto", rol.DescripcionRol);
                        mscom.Parameters.AddWithValue("@codigoestado", rol.Estado);
                        mscom.Parameters.AddWithValue("@fechacreacion", DateTime.Now.ToString("yyyy-MM-dd"));
                        mscom.Parameters.AddWithValue("@fechamodificacion", rol.FechaModificacion);                   
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
        public async Task<bool> Editar(Roles rol)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Update adm_cat_roles SET
                                nombrerol = @nombrerol,
                                descripcionrol =@descripcionrol,
                                estado = @estado,
                                fechacreacion =@fechacreacion,
                                fechamodificacion=@fechamodificacion                                
                                WHERE
                                codigorol = @codigorol;
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@codigorol", rol.CodigoRol);
                        mscom.Parameters.AddWithValue("@nombrerol", rol.NombreRol);
                        mscom.Parameters.AddWithValue("@descripcionrol", rol.DescripcionRol);
                        mscom.Parameters.AddWithValue("@codigoestado", rol.Estado);
                        mscom.Parameters.AddWithValue("@fechacreacion", rol.FechaCreacion);
                        mscom.Parameters.AddWithValue("@fechamodificacion", rol.FechaModificacion);                        
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
