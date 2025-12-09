using Microsoft.AspNetCore.Mvc;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClientesService _clientesService;

        public ClientesController(IClientesService clientesService)
        {
            _clientesService = clientesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            try
            {
                var clientes = await _clientesService.ObtenerClientes();
                return Ok(clientes);
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al obtener los clientes. Intenta nuevamente." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            try
            {
                var cliente = await _clientesService.ObtenerClientePorId(id);
                if (cliente == null)
                    return NotFound(new { mensaje = "Cliente no encontrado." });

                return Ok(cliente);
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al obtener el cliente. Intenta nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] ClienteCreateDto dto)
        {
            try
            {
                var clienteCreado = await _clientesService.CrearCliente(dto);
                return CreatedAtAction(nameof(GetCliente),
                    new { id = clienteCreado.IdCliente },
                    clienteCreado);
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al crear el cliente. Intenta nuevamente." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ClienteCreateDto dto)
        {
            try
            {
                var clienteActualizado = await _clientesService.ActualizarCliente(id, dto);
                if (clienteActualizado == null)
                    return NotFound(new { mensaje = "Cliente no encontrado." });

                return NoContent();
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al actualizar el cliente. Intenta nuevamente." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            try
            {
                var result = await _clientesService.EliminarCliente(id);
                if (!result)
                    return NotFound(new { mensaje = "Cliente no encontrado." });

                return NoContent();
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al eliminar el cliente. Intenta nuevamente." });
            }
        }
    }
}

