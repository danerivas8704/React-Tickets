using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Tickets.Modelo;


namespace Tickets.Datos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        Modelo.Utilidades.Utilidades util = new Modelo.Utilidades.Utilidades();

        public UsuariosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            string consulta = @"DELETE FROM adm_cat_usuarios WHERE codigousuario = @id";
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
                                message = "Usuario eliminado correctamente."
                            });
                        }
                        else
                        {
                            return NotFound(new
                            {
                                success = false,
                                message = "No se encontró el usuario."
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
                    message = "Ocurrió un error al eliminar el usuario.",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("ObtenerC")]
        public async Task<List<Catalogo>> ObtenerTodosCombo()
        {
            string consulta = @"Select codigousuario,nombreusuario from adm_cat_usuarios";
            List<Catalogo> lstUsuarios = new List<Catalogo>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        using (var drUsuarios = await mscom.ExecuteReaderAsync())
                        {
                            while (drUsuarios.Read())
                            {
                                lstUsuarios.Add(new Catalogo
                                {
                                    Id = Convert.ToInt32(drUsuarios["codigousuario"]),
                                    Nombre = drUsuarios["nombreusuario"].ToString()
                                    
                                });
                            }
                            drUsuarios.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstUsuarios;
        }


        [HttpGet]
        [Route("ObtenerCodigo/{id:int}")]
        public async Task<List<Usuarios>> ObtenerCodigo(int id)
        {
            string consulta = @"Select * from adm_cat_usuarios where codigousuario=@id";
            List<Usuarios> lstUsuarios = new List<Usuarios>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();

                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@id", id);
                        using (var drUsuarios = await mscom.ExecuteReaderAsync())
                        {
                            while (drUsuarios.Read())
                            {
                                lstUsuarios.Add(new Usuarios
                                {
                                    CodigoUsuario = Convert.ToInt32(drUsuarios["codigousuario"]),
                                    NombreUsuario = drUsuarios["nombreusuario"].ToString(),
                                    ApellidoUsuario = drUsuarios["apellidousuario"].ToString(),
                                    Correo = drUsuarios["correo"].ToString(),
                                    Direccion = drUsuarios["direccion"].ToString(),
                                    CodigoEstado = Convert.ToInt32(drUsuarios["codigoestado"]),
                                    CodigoDepto = Convert.ToInt32(drUsuarios["codigodepto"]),
                                    Password = drUsuarios["password"].ToString(),
                                    FechaCreacion = Convert.ToDateTime(drUsuarios["fechacreacion"]),
                                    FechaModificacion = Convert.ToDateTime(drUsuarios["fechamodificacion"]),
                                    UltimoLogin = Convert.ToDateTime(drUsuarios["ultimologin"])
                                });
                            }
                            drUsuarios.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstUsuarios;
        }

        [HttpGet]
        [Route("Obtener")]
        public async Task<List<Usuarios>> ObtenerTodos()
        {
            string consulta = @"Select * from adm_cat_usuarios";
            List<Usuarios> lstUsuarios = new List<Usuarios>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        using (var drUsuarios = await mscom.ExecuteReaderAsync())
                        {
                            while (drUsuarios.Read())
                            {
                                lstUsuarios.Add(new Usuarios
                                {
                                    CodigoUsuario = Convert.ToInt32(drUsuarios["codigousuario"]),
                                    NombreUsuario = drUsuarios["nombreusuario"].ToString(),
                                    ApellidoUsuario = drUsuarios["apellidousuario"].ToString(),
                                    Correo = drUsuarios["correo"].ToString(),
                                    Direccion = drUsuarios["direccion"].ToString(),
                                    CodigoEstado = Convert.ToInt32(drUsuarios["codigoestado"]),
                                    CodigoDepto = Convert.ToInt32(drUsuarios["codigodepto"]),
                                    Password = drUsuarios["password"].ToString(),
                                    FechaCreacion = Convert.ToDateTime(drUsuarios["fechacreacion"]),
                                    FechaModificacion = Convert.ToDateTime(drUsuarios["fechamodificacion"]),
                                    UltimoLogin = Convert.ToDateTime(drUsuarios["ultimologin"])
                                });
                            } 
                            drUsuarios.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstUsuarios;
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<bool> Crear(Usuarios usuario)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Insert into adm_cat_usuarios(nombreusuario,apellidousuario,
                                correo, direccion,codigoestado,codigodepto, password,
                                fechacreacion,fechamodificacion,ultimologin)
                                values
                                (@nombreusuario,@apellidousuario,@correo,
                                @direccion,@codigoestado,@codigodepto,@password,
                                @fechacreacion,@fechamodificacion,@ultimologin)
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@nombreusuario", usuario.NombreUsuario);
                        mscom.Parameters.AddWithValue("@apellidousuario", usuario.ApellidoUsuario);
                        mscom.Parameters.AddWithValue("@correo", usuario.Correo);
                        mscom.Parameters.AddWithValue("@direccion", usuario.Direccion);
                        mscom.Parameters.AddWithValue("@codigoestado", usuario.CodigoEstado);
                        mscom.Parameters.AddWithValue("@codigodepto", usuario.CodigoDepto);
                        mscom.Parameters.AddWithValue("@password", util.encriptarSHA256(usuario.Password));
                        mscom.Parameters.AddWithValue("@fechacreacion", DateTime.Now.ToString("yyyy-MM-dd"));
                        mscom.Parameters.AddWithValue("@fechamodificacion", usuario.FechaModificacion);
                        mscom.Parameters.AddWithValue("@ultimologin", usuario.UltimoLogin);
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
        [Route("Cambio")]
        public async Task<bool> CambioPass(string usuario, string password)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Update adm_cat_usuarios SET                              
                                password=@password                         
                                WHERE
                                nombreusuario = @nombreusuario;
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@nombreusuario", usuario);                      
                        mscom.Parameters.AddWithValue("@password", password);                 
                        mscom.Parameters.AddWithValue("@fechamodificacion", DateTime.Now.ToString("yyyy-MM-dd"));                      
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
        public async Task<bool> Editar(Usuarios usuario)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Update adm_cat_usuarios SET
                                nombreusuario = @nombreusuario,
                                apellidousuario=@apellidousuario,
                                correo = @correo,
                                direccion=@direccion,
                                codigoestado=@codigoestado,
                                codigodepto=@codigodepto,
                                password=@password,
                                fechacreacion=@fechacreacion,
                                fechamodificacion=@fechamodificacion,
                                ultimologin=@ultimologin
                                WHERE
                                codigousuario = @codigousuario;
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@codigousuario", usuario.CodigoUsuario);
                        mscom.Parameters.AddWithValue("@nombreusuario", usuario.NombreUsuario);
                        mscom.Parameters.AddWithValue("@apellidousuario", usuario.ApellidoUsuario);
                        mscom.Parameters.AddWithValue("@correo", usuario.Correo);
                        mscom.Parameters.AddWithValue("@direccion", usuario.Direccion);
                        mscom.Parameters.AddWithValue("@codigoestado", usuario.CodigoEstado);
                        mscom.Parameters.AddWithValue("@codigodepto", usuario.CodigoDepto);
                        mscom.Parameters.AddWithValue("@password", usuario.Password);
                        mscom.Parameters.AddWithValue("@fechacreacion", DateTime.Now.ToString("yyyy-MM-dd"));
                        mscom.Parameters.AddWithValue("@fechamodificacion", usuario.FechaModificacion);
                        mscom.Parameters.AddWithValue("@ultimologin", usuario.UltimoLogin);
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
