using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using Tickets.Seguridad.Modelo;
using Tickets.Seguridad.Modelo.DTO;
using Tickets.Seguridad.Utilidades;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Tickets.Seguridad.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly Utilidades.Utilidades _utilidades;
        private readonly IConfiguration _configuration;
        public AccesoController(Utilidades.Utilidades utilidades, IConfiguration configuration)
        {
            _utilidades = utilidades;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO objeto)
        {
            var modeloUsuario = new Usuario
            {
                NombreUsuario = objeto.Nombre,
                CorreoUsuario = objeto.Correo,
                ClaveUsuario = _utilidades.encriptarSHA256(objeto.Clave)
            };

            //Inserta registro ADO.NET


            //

            if (modeloUsuario.CodigoUsuario != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO objeto)
        {
            string consulta = @"SELECT nombreusuario, password 
                        FROM adm_cat_usuarios 
                        WHERE nombreusuario = @usuario;";

            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                    {
                        cmd.Parameters.AddWithValue("@usuario", objeto.Usuario);

                        using (var dr = await cmd.ExecuteReaderAsync())
                        {
                            if (await dr.ReadAsync())
                            {
                                string passwordDb = dr["password"].ToString(); 

                                // Validar contraseña (plaintext o con hash)
                                if (passwordDb == _utilidades.encriptarSHA256(objeto.Clave))
                                {
                                    var usuario = new LoginDTO
                                    {
                                        Usuario = dr["nombreusuario"].ToString(),
                                        Clave = dr["password"].ToString()
                                    };

                                    // 🔹 Generar el token JWT usando el método generarJWT(objeto)
                                    var token = _utilidades.generarJWT(usuario);
                                    return Ok(new
                                    {
                                        success = true,
                                        message = "Inicio de sesión exitoso.",
                                        data = usuario,
                                        token = token
                                    });
                                }
                                else
                                {
                                    return Unauthorized(new
                                    {
                                        success = false,
                                        message = "Contraseña incorrecta."
                                    });
                                }
                            }
                            else
                            {
                                return NotFound(new
                                {
                                    success = false,
                                    message = "Usuario no encontrado."
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor.",
                    error = ex.Message
                });
            }
        }

    }
}