using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================
        // GET: api/Servicios
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetServicios()
        {
            var servicios = await _context.Servicios.ToListAsync();
            return Ok(servicios);
        }

        // ============================================
        // GET: api/Servicios/{id}
        // ============================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServicio(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            return Ok(servicio);
        }

        // ============================================
        // POST: api/Servicios
        // ============================================
        [HttpPost]
        public async Task<IActionResult> CrearServicio([FromBody] Servicios dto)
        {
            _context.Servicios.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServicio), new { id = dto.IdServicio }, dto);
        }

        // ============================================
        // PUT: api/Servicios/{id}
        // ============================================
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarServicio(int id, [FromBody] Servicios dto)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            servicio.NombreServicio = dto.NombreServicio;
            servicio.Precio = dto.Precio;
            servicio.DuracionMin = dto.DuracionMin;
            servicio.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ============================================
        // DELETE: api/Servicios/{id}
        // ============================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarServicio(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
