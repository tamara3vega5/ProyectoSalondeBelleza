using Microsoft.AspNetCore.Mvc;
using Salon_Api.DTO;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentasService _ventasService;

        public VentasController(IVentasService ventasService)
        {
            _ventasService = ventasService;
        }

        // POST: api/Ventas/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaCreateDto dto)
        {
            try
            {
                // Llama al servicio para crear la venta
                var ventaCreada = await _ventasService.CrearVenta(dto);

                if (ventaCreada == null)
                    return BadRequest(new { mensaje = "No se pudo registrar la venta" });

                return Ok(new
                {
                    mensaje = "Venta registrada correctamente",
                    venta = ventaCreada
                });
            }
            catch (Exception ex)
            {
                // Captura inner exception para depuración detallada
                return StatusCode(500, new
                {
                    message = "Error al crear la venta",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        // GET: api/Ventas
        [HttpGet]
        public async Task<IActionResult> ObtenerVentas()
        {
            var ventas = await _ventasService.ObtenerVentas();
            return Ok(ventas);
        }

        // GET: api/Ventas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerVenta(int id)
        {
            var venta = await _ventasService.ObtenerVenta(id);
            if (venta == null)
                return NotFound(new { mensaje = "Venta no encontrada" });

            return Ok(venta);
        }
    }
}

//////