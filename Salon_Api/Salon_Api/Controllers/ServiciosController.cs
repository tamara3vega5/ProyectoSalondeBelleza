using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly IServiciosService _service;

        public ServiciosController(IServiciosService service)
        {
            _service = service;
        }

        // ==========================================================
        // GET: api/Servicios
        // ==========================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioReadDto>>> GetServicios()
        {
            try
            {
                var datos = await _service.ObtenerServicios();

                var lista = datos.Select(s => new ServicioReadDto
                {
                    IdServicio = s.IdServicio,
                    NombreServicio = s.NombreServicio,
                    Descripcion = s.Descripcion,
                    DuracionMin = s.DuracionMin,
                    Precio = s.Precio
                });

                return Ok(lista);
            }
            catch
            {
                return BadRequest(new { mensaje = "Error al obtener los servicios." });
            }
        }

        // ==========================================================
        // GET: api/Servicios/5
        // ==========================================================
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioReadDto>> GetServicio(int id)
        {
            try
            {
                var s = await _service.ObtenerServicio(id);
                if (s == null)
                    return NotFound(new { mensaje = "Servicio no encontrado." });

                return Ok(new ServicioReadDto
                {
                    IdServicio = s.IdServicio,
                    NombreServicio = s.NombreServicio,
                    Descripcion = s.Descripcion,
                    DuracionMin = s.DuracionMin,
                    Precio = s.Precio
                });
            }
            catch
            {
                return BadRequest(new { mensaje = "Error al obtener el servicio." });
            }
        }

        // ==========================================================
        // POST: api/Servicios
        // ==========================================================
        [HttpPost]
        public async Task<ActionResult<ServicioReadDto>> PostServicio([FromBody] ServicioCreateDto dto)
        {
            try
            {
                var servicio = new Servicios
                {
                    NombreServicio = dto.NombreServicio,
                    Descripcion = dto.Descripcion,
                    DuracionMin = dto.DuracionMin,
                    Precio = dto.Precio
                };

                var creado = await _service.CrearServicio(servicio);

                var readDto = new ServicioReadDto
                {
                    IdServicio = creado.IdServicio,
                    NombreServicio = creado.NombreServicio,
                    Descripcion = creado.Descripcion,
                    DuracionMin = creado.DuracionMin,
                    Precio = creado.Precio
                };

                return CreatedAtAction(nameof(GetServicio), new { id = creado.IdServicio }, readDto);
            }
            catch
            {
                return BadRequest(new { mensaje = "Error al crear el servicio." });
            }
        }


        // ==========================================================
        // PUT: api/Servicios/5
        // ==========================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicio(int id, [FromBody] ServicioUpdateDto dto)
        {
            try
            {
                var nuevo = new Servicios
                {
                    NombreServicio = dto.NombreServicio,
                    Descripcion = dto.Descripcion,
                    DuracionMin = dto.DuracionMin,
                    Precio = dto.Precio
                };

                var ok = await _service.ActualizarServicio(id, nuevo);

                if (!ok)
                    return NotFound(new { mensaje = "Servicio no encontrado." });

                return NoContent();
            }
            catch
            {
                return BadRequest(new { mensaje = "Error al actualizar el servicio." });
            }
        }


        // ==========================================================
        // DELETE: api/Servicios/5
        // ==========================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            try
            {
                var ok = await _service.EliminarServicio(id);

                if (!ok)
                    return NotFound(new { mensaje = "Servicio no encontrado." });

                return NoContent();
            }
            catch
            {
                return BadRequest(new { mensaje = "Error al eliminar el servicio." });
            }
        }
    }
}
