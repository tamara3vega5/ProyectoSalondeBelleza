using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstilistasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstilistasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================
        // GET: api/Estilistas
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetEstilistas()
        {
            var estilistas = await _context.Estilistas.ToListAsync();
            return Ok(estilistas);
        }

        // ============================================
        // GET: api/Estilistas/{id}
        // ============================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstilista(int id)
        {
            var estilista = await _context.Estilistas.FindAsync(id);
            if (estilista == null)
                return NotFound(new { mensaje = "Estilista no encontrado" });

            return Ok(estilista);
        }

        // ============================================
        // POST: api/Estilistas
        // ============================================
        [HttpPost]
        public async Task<IActionResult> CrearEstilista([FromBody] Estilistas dto)
        {
            _context.Estilistas.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstilista), new { id = dto.IdEstilista }, dto);
        }

        // ============================================
        // PUT: api/Estilistas/{id}
        // ============================================
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEstilista(int id, [FromBody] Estilistas dto)
        {
            var estilista = await _context.Estilistas.FindAsync(id);
            if (estilista == null)
                return NotFound(new { mensaje = "Estilista no encontrado" });

            estilista.Nombre = dto.Nombre;
            estilista.Telefono = dto.Telefono;
            estilista.Correo = dto.Correo;
            estilista.Especialidad = dto.Especialidad;
            estilista.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ============================================
        // DELETE: api/Estilistas/{id}
        // ============================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEstilista(int id)
        {
            var estilista = await _context.Estilistas.FindAsync(id);
            if (estilista == null)
                return NotFound(new { mensaje = "Estilista no encontrado" });

            _context.Estilistas.Remove(estilista);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
