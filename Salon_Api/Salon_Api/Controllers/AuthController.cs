using Microsoft.AspNetCore.Mvc;
using Salon_Api.DTO;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var cliente = await _authService.Login(dto);

                if (cliente == null)
                    return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });

                // Construimos el DTO de respuesta
                var response = new LoginResponseDto
                {
                    IdCliente = cliente.IdCliente,
                    Nombre = cliente.Nombre,
                    Rol = cliente.Rol ?? "cliente",   // por si viene null
                    Token = "" // si luego implementas JWT, aquí se agrega
                };

                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso",
                    usuario = response
                });
            }
            catch (Exception)
            {
                return BadRequest(new { mensaje = "Ocurrió un error al intentar iniciar sesión." });
            }
        }
    }
}
