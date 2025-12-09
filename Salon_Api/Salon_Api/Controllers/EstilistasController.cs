using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstilistasController : ControllerBase
    {
        private readonly IEstilistasService _service;

        public EstilistasController(IEstilistasService service)
        {
            _service = service;
        }

        // GET: api/Estilistas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstilistaReadDto>>> GetEstilistas()
        {
            var datos = await _service.ObtenerEstilistas();

            var lista = datos.Select(e => new EstilistaReadDto
            {
                IdEstilista = e.IdEstilista,
                Nombre = e.Nombre,
                Especialidad = e.Especialidad,
                Telefono = e.Telefono,
                Correo = e.Correo
            });

            return Ok(lista);
        }

        // GET: api/Estilistas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstilistaReadDto>> GetEstilista(int id)
        {
            var e = await _service.ObtenerEstilista(id);

            if (e == null)
                return NotFound();

            return Ok(new EstilistaReadDto
            {
                IdEstilista = e.IdEstilista,
                Nombre = e.Nombre,
                Especialidad = e.Especialidad,
                Telefono = e.Telefono,
                Correo = e.Correo
            });
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> PostEstilista(EstilistaCreateDto dto)
        {
            var nuevo = new Estilistas
            {
                Nombre = dto.Nombre,
                Especialidad = dto.Especialidad,
                Telefono = dto.Telefono,
                Correo = dto.Correo
            };

            var creado = await _service.CrearEstilista(nuevo);

            return CreatedAtAction(nameof(GetEstilista), new { id = creado.IdEstilista }, creado);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEstilista(int id, EstilistaUpdateDto dto)
        {
            var nuevo = new Estilistas
            {
                Nombre = dto.Nombre,
                Especialidad = dto.Especialidad,
                Telefono = dto.Telefono,
                Correo = dto.Correo
            };

            var ok = await _service.ActualizarEstilista(id, nuevo);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEstilista(int id)
        {
            var ok = await _service.EliminarEstilista(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}
