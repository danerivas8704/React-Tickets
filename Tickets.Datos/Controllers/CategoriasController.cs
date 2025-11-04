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
    public class CategoriasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CategoriasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ObtenerC")]
        public async Task<List<Catalogo>> ObtenerTodosCombo()
        {
            string consulta = @"Select codigocategoria,nombrecategoria from adm_cat_categoria";
            List<Catalogo> lstCategorias = new List<Catalogo>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        using (var drCategorias = await mscom.ExecuteReaderAsync())
                        {
                            while (drCategorias.Read())
                            {
                                lstCategorias.Add(new Catalogo
                                {
                                    Id = Convert.ToInt32(drCategorias["codigocategoria"]),
                                    Nombre = drCategorias["nombrecategoria"].ToString()

                                });
                            }
                            drCategorias.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstCategorias;
        }

        [HttpGet]
        [Route("ObtenerCodigo/{id:int}")]
        public async Task<List<Categorias>> ObtenerCodigo(int id)
        {
            string consulta = @"Select * from adm_cat_categoria where codigocategoria=@id";
            List<Categorias> lstCategorias = new List<Categorias>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();

                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@id", id);
                        using (var drCategorias = await mscom.ExecuteReaderAsync())
                        {
                            while (drCategorias.Read())
                            {
                                lstCategorias.Add(new Categorias
                                {
                                    CodigoCategoria = Convert.ToInt32(drCategorias["codigocategoria"]),
                                    NombreCategoria = drCategorias["nombrecategoria"].ToString(),
                                    DescripcionCategoria = drCategorias["descripcioncategoria"].ToString(),                                    
                                    CodigoEstado = Convert.ToInt32(drCategorias["codigoestado"])
                                });
                            }
                            drCategorias.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstCategorias;
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            string consulta = @"DELETE FROM adm_cat_categoria WHERE codigocategoria = @id";
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
                                message = "Categoría eliminada correctamente."
                            });
                        }
                        else
                        {
                            return NotFound(new
                            {
                                success = false,
                                message = "No se encontró la categoría especificada."
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
                    message = "Ocurrió un error al eliminar la categoría.",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("Obtener")]
        public async Task<List<Categorias>> ObtenerTodos()
        {
            string consulta = @"Select * from adm_cat_categoria";
            List<Categorias> lstCategorias = new List<Categorias>();
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        using (var drCategorias = await mscom.ExecuteReaderAsync())
                        {
                            while (drCategorias.Read())
                            {
                                lstCategorias.Add(new Categorias
                                {
                                    CodigoCategoria = Convert.ToInt32(drCategorias["codigocategoria"]),
                                    NombreCategoria = drCategorias["nombrecategoria"].ToString(),
                                    DescripcionCategoria = drCategorias["descripcioncategoria"].ToString(),
                                    CodigoEstado = Convert.ToInt32(drCategorias["codigoestado"])                                    
                                });
                            }
                            drCategorias.Close();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);

            }
            return lstCategorias;
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<bool> Crear(Categorias categoria)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Insert into adm_cat_categoria(nombrecategoria,descripcioncategoria,
                                codigoestado)
                                values
                                (@nombrecategoria,@descripcioncategoria,@codigoestado
                                )
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@nombrecategoria", categoria.NombreCategoria);
                        mscom.Parameters.AddWithValue("@descripcioncategoria", categoria.DescripcionCategoria);
                        mscom.Parameters.AddWithValue("@codigoestado", categoria.CodigoEstado);                        
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
        public async Task<bool> Editar(Categorias categoria)
        {
            bool blnRespuesta = true;
            string sqlDs = _configuration.GetConnectionString("CadenaMysql");

            string consulta = @"Update adm_cat_categoria SET
                                nombrecategoria = @nombrecategoria,
                                descripcioncategoria =@descripcioncategoria,
                                codigoestado = @codigoestado                                                  
                                WHERE
                                codigocategoria = @codigocategoria;
                              ";
            try
            {
                using (MySqlConnection con = new MySqlConnection(sqlDs))
                {
                    await con.OpenAsync();
                    using (MySqlCommand mscom = new MySqlCommand(consulta, con))
                    {
                        mscom.Parameters.AddWithValue("@codigocategoria", categoria.CodigoCategoria);
                        mscom.Parameters.AddWithValue("@nombrecategoria", categoria.NombreCategoria);
                        mscom.Parameters.AddWithValue("@descripcioncategoria", categoria.DescripcionCategoria);
                        mscom.Parameters.AddWithValue("@codigoestado", categoria.CodigoEstado);                        
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
