using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tickets.Seguridad.Modelo;
using Tickets.Seguridad.Modelo.DTO;
using Tickets.Seguridad.Utilidades;
using Microsoft.AspNetCore.Authorization;


namespace Tickets.Seguridad.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly Utilidades.Utilidades _utilidades;
        public AccesoController(Utilidades.Utilidades utilidades)
        {
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]        
        public async Task<IActionResult>Registrarse(UsuarioDTO objeto)
        {
            var modeloUsuario = new Usuario
            {
                NombreUsuario = objeto.Nombre,
                CorreoUsuario = objeto.Correo,
                ClaveUsuario = _utilidades.encriptarSHA256(objeto.Clave)
            };

            //Inserta registro ADO.NET

            if (modeloUsuario.CodigoUsuario != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });

        }

        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult>Login(LoginDTO objeto)
        //{
            //Hacer select para obtener si el usuario existe

            //si usuario existe entonces
        //}

    }
}
