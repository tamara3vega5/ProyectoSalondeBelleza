using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClientesService _service;

        public ClientesController(IClientesService service)
        {
            _service = service;
        }

        // ============================================================
        // GET TODOS
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteReadDto>>> GetClientes()
        {
            var data = await _service.ObtenerClientes();

            return Ok(data.Select(c => new ClienteReadDto
            {
                IdCliente = c.IdCliente,
                Nombre = c.Nombre,
                Telefono = c.Telefono,
                Correo = c.Correo,
                Rol = c.Rol
            }));
        }

        // ============================================================
        // GET POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteReadDto>> GetCliente(int id)
        {
            var c = await _service.ObtenerCliente(id);
            if (c == null) return NotFound();

            return Ok(new ClienteReadDto
            {
                IdCliente = c.IdCliente,
                Nombre = c.Nombre,
                Telefono = c.Telefono,
                Correo = c.Correo,
                Rol = c.Rol
            });
        }

        // ============================================================
        // POST: REGISTRAR CLIENTE
        // ============================================================
        [HttpPost]
        public async Task<ActionResult> PostCliente(ClienteCreateDto dto)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

            var nuevo = new Clientes
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Correo = dto.Correo?.Trim().ToLower(),
                PasswordHash = hash,
                Rol = dto.Rol
            };

            var creado = await _service.CrearCliente(nuevo);

            return CreatedAtAction(nameof(GetCliente), new { id = creado.IdCliente }, creado);
        }

        // ============================================================
        // PUT: ACTUALIZAR CLIENTE
        // ============================================================
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCliente(int id, ClienteUpdateDto dto)
        {
            string? newHash = null;

            if (!string.IsNullOrWhiteSpace(dto.NuevaContrasena))
                newHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevaContrasena);

            var nuevo = new Clientes
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Correo = dto.Correo?.Trim().ToLower(),
                PasswordHash = newHash ?? "",
                Rol = dto.Rol ?? ""
            };

            var ok = await _service.ActualizarCliente(id, nuevo);

            if (!ok) return NotFound();
            return NoContent();
        }

        // ============================================================
        // DELETE
        // ============================================================
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            try
            {
                var ok = await _service.EliminarCliente(id);
                if (!ok) return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // ============================================================
        // CAMBIAR CONTRASEÑA
        // ============================================================
        [HttpPut("{id}/password")]
        public async Task<IActionResult> CambiarPassword(int id, [FromBody] string nueva)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(nueva);

            var ok = await _service.CambiarPassword(id, hash);
            if (!ok) return NotFound();

            return NoContent();
        }

        // ============================================================
        // CAMBIAR ROL (admin / cliente)
        // ============================================================
        [HttpPut("{id}/rol")]
        public async Task<IActionResult> CambiarRol(int id, [FromBody] string rol)
        {
            var ok = await _service.CambiarRol(id, rol);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}
