using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductosService _service;

        public ProductosController(IProductosService service)
        {
            _service = service;
        }

        // =================== GET (lista) ===================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoReadDto>>> GetProductos()
        {
            var data = await _service.ObtenerProductos();

            var lista = data.Select(p => new ProductoReadDto
            {
                IdProducto = p.IdProducto,
                NombreProducto = p.NombreProducto,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock
            });

            return Ok(lista);
        }

        // =================== GET (uno) ===================
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoReadDto>> GetProducto(int id)
        {
            var p = await _service.ObtenerProducto(id);

            if (p == null)
                return NotFound();

            return Ok(new ProductoReadDto
            {
                IdProducto = p.IdProducto,
                NombreProducto = p.NombreProducto,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock
            });
        }

        // =================== POST ===================
        [HttpPost]
        public async Task<ActionResult> PostProducto(ProductoCreateDto dto)
        {
            var nuevo = new Productos
            {
                NombreProducto = dto.NombreProducto,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock
            };

            var creado = await _service.CrearProducto(nuevo);

            return CreatedAtAction(nameof(GetProducto), new { id = creado.IdProducto }, creado);
        }

        // =================== PUT ===================
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProducto(int id, ProductoUpdateDto dto)
        {
            var nuevo = new Productos
            {
                NombreProducto = dto.NombreProducto,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock
            };

            var ok = await _service.ActualizarProducto(id, nuevo);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        // =================== DELETE ===================
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProducto(int id)
        {
            var ok = await _service.EliminarProducto(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}

