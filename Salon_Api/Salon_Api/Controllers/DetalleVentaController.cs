using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentaController : ControllerBase
    {
        private readonly IDetalleVentaService _detallesService;

        public DetalleVentaController(IDetalleVentaService detallesService)
        {
            _detallesService = detallesService;
        }

        // GET: api/DetalleVenta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleVentaDto>>> GetDetalles()
        {
            var lista = await _detallesService.ObtenerDetalles();

            var dto = lista.Select(d => new DetalleVentaDto
            {
                IdDetalle = d.IdDetalle,
                IdProducto = d.IdProducto,
                NombreProducto = d.Producto?.NombreProducto ?? "",
                Cantidad = d.Cantidad,
                Subtotal = d.Subtotal
            });

            return Ok(dto);
        }

        // GET: api/DetalleVenta/venta/5
        [HttpGet("venta/{idVenta}")]
        public async Task<ActionResult<IEnumerable<DetalleVentaDto>>> GetPorVenta(int idVenta)
        {
            var lista = await _detallesService.ObtenerPorVenta(idVenta);

            var dto = lista.Select(d => new DetalleVentaDto
            {
                IdDetalle = d.IdDetalle,
                IdProducto = d.IdProducto,
                NombreProducto = d.Producto?.NombreProducto ?? "",
                Cantidad = d.Cantidad,
                Subtotal = d.Subtotal
            });

            return Ok(dto);
        }

        // GET: api/DetalleVenta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleVentaDto>> GetDetalle(int id)
        {
            var d = await _detallesService.ObtenerDetalle(id);
            if (d == null)
                return NotFound();

            return Ok(new DetalleVentaDto
            {
                IdDetalle = d.IdDetalle,
                IdProducto = d.IdProducto,
                NombreProducto = d.Producto?.NombreProducto ?? "",
                Cantidad = d.Cantidad,
                Subtotal = d.Subtotal
            });
        }

        // POST: api/DetalleVenta
        [HttpPost]
        public async Task<ActionResult> PostDetalle([FromBody] DetalleVentaCreateDto dto)
        {
            var detalle = new DetalleVenta
            {
                IdProducto = dto.IdProducto,
                Cantidad = dto.Cantidad,
                Subtotal = 0 // Se recalcula por seguridad
            };

            var creado = await _detallesService.CrearDetalle(detalle);

            return CreatedAtAction(nameof(GetDetalle), new { id = creado.IdDetalle }, creado);
        }

        // DELETE: api/DetalleVenta/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDetalle(int id)
        {
            var ok = await _detallesService.EliminarDetalle(id);

            if (!ok) return NotFound();
            return NoContent();
        }
    }
}

