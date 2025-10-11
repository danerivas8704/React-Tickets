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
    public class DepartamentosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartamentosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Obtener")]
        public async Task<List<Departamentos>> ObtenerTodos()
        {
            string consulta = @"Select * from adm_cat_clientes";
            List<Departamentos> lstDepartamentos = new List<Departamentos>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        using (var drDeptos = await mscom.ExecuteReaderAsync())
                        {
                            while (drDeptos.Read())
                            {
                                lstDepartamentos.Add(new Departamentos
                                {
                                    CodigoDepto = Convert.ToInt32(drDeptos["codigodepto"]),
                                    NombreDepto = drDeptos["nombredepto"].ToString(),
                                    DescripcionDepto = drDeptos["descripciondepto"].ToString(),
                                    FechaCreacion = Convert.ToDateTime(drDeptos["fechacreacion"]),
                                    FechaModificacion = Convert.ToDateTime(drDeptos["fechamodificacion"]),
                                    CodigoEstado = Convert.ToInt32(drDeptos["codigoestado"])
                                });
                            }
                            drDeptos.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstDepartamentos;
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<bool> Crear(Departamentos departamento)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Insert into adm_cat_depto(nombredepto,descripciondepto,
                                fechacreacion, fechamodificacion, codigoestado)
                                values
                                (@nombredepto,@descripciondepto,@fechacreacion,
                                @fechamodificacion,@codigoestado)
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@nombredepto", departamento.NombreDepto);
                        mscom.Parameters.AddWithValue("@descripciondepto", departamento.DescripcionDepto);
                        mscom.Parameters.AddWithValue("@fechacreacion", DateTime.Now.ToString("yyyy-MM-dd"));
                        mscom.Parameters.AddWithValue("@fechamodificacion", departamento.FechaModificacion);
                        mscom.Parameters.AddWithValue("@codigoestado", departamento.CodigoEstado);
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
        public async Task<bool> Editar(Departamentos departamento)
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
                        mscom.Parameters.AddWithValue("@codigodepto", departamento.CodigoDepto);
                        mscom.Parameters.AddWithValue("@nombredepto", departamento.NombreDepto);
                        mscom.Parameters.AddWithValue("@descripciondepto", departamento.DescripcionDepto);
                        mscom.Parameters.AddWithValue("@fechacreacion", departamento.FechaCreacion);
                        mscom.Parameters.AddWithValue("@fechamodificacion", departamento.FechaModificacion);
                        mscom.Parameters.AddWithValue("@codigoestado", departamento.CodigoEstado);
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
