using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentasService _ventasService;
        private readonly IProductosService _productosService;
        private readonly IClientesService _clientesService;

        public VentasController(
            IVentasService ventasService,
            IProductosService productosService,
            IClientesService clientesService)
        {
            _ventasService = ventasService;
            _productosService = productosService;
            _clientesService = clientesService;
        }

        // ===================== GET LISTA =====================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaReadDto>>> GetVentas()
        {
            var ventas = await _ventasService.ObtenerVentas();

            var result = ventas.Select(v => new VentaReadDto
            {
                IdVenta = v.IdVenta,
                IdCliente = v.IdCliente,
                NombreCliente = v.Cliente.Nombre,
                Fecha = v.Fecha,
                Total = v.Total,
                Detalles = v.DetalleVentas.Select(d => new DetalleVentaDto
                {
                    IdDetalle = d.IdDetalle,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.Producto.NombreProducto,
                    Cantidad = d.Cantidad,
                    Subtotal = d.Subtotal
                }).ToList()
            });

            return Ok(result);
        }

        // ===================== GET POR ID =====================
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaReadDto>> GetVenta(int id)
        {
            var v = await _ventasService.ObtenerVenta(id);

            if (v == null)
                return NotFound();

            return Ok(new VentaReadDto
            {
                IdVenta = v.IdVenta,
                IdCliente = v.IdCliente,
                NombreCliente = v.Cliente.Nombre,
                Fecha = v.Fecha,
                Total = v.Total,
                Detalles = v.DetalleVentas.Select(d => new DetalleVentaDto
                {
                    IdDetalle = d.IdDetalle,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.Producto.NombreProducto,
                    Cantidad = d.Cantidad,
                    Subtotal = d.Subtotal
                }).ToList()
            });
        }

        // ===================== POST (CREAR VENTA) =====================
        [HttpPost]
        public async Task<ActionResult> PostVenta(VentaCreateDto dto)
        {
            var venta = new Ventas
            {
                IdCliente = dto.IdCliente,
                Fecha = DateTime.Now,
                Total = dto.Total
            };

            var detalles = new List<DetalleVenta>();

            foreach (var d in dto.Detalles)
            {
                var prod = await _productosService.ObtenerProducto(d.IdProducto);
                if (prod == null)
                    return BadRequest($"Producto {d.IdProducto} no existe.");

                detalles.Add(new DetalleVenta
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    Subtotal = prod.Precio * d.Cantidad
                });
            }

            var creada = await _ventasService.CrearVenta(venta, detalles);

            return CreatedAtAction(nameof(GetVenta), new { id = creada.IdVenta }, creada);
        }

        // ===================== DELETE =====================
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVenta(int id)
        {
            var ok = await _ventasService.EliminarVenta(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}
