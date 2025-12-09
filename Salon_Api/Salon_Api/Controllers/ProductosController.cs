using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================
        // GET: api/Productos
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            var productos = await _context.Productos.ToListAsync();
            return Ok(productos);
        }

        // ============================================
        // GET: api/Productos/{id}
        // ============================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            return Ok(producto);
        }

        // ============================================
        // POST: api/Productos
        // ============================================
        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] Productos dto)
        {
            _context.Productos.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = dto.IdProducto }, dto);
        }

        // ============================================
        // PUT: api/Productos/{id}
        // ============================================
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Productos dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            producto.NombreProducto = dto.NombreProducto;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
            producto.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ============================================
        // DELETE: api/Productos/{id}
        // ============================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
