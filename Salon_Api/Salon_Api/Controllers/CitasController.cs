using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;
using Salon_Api.Modelo;
using Salon_Api.Data;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ICitasService _service;
        private readonly ApplicationDbContext _context;

        public CitasController(ICitasService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // ===================== GET TODAS =====================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaReadDto>>> GetCitas()
        {
            var citas = await _service.ObtenerCitas();

            var resultado = citas.Select(c => new CitaReadDto
            {
                IdCita = c.IdCita,
                Fecha = c.Fecha,
                Estado = c.Estado,

                Cliente = new ClienteMiniDto
                {
                    IdCliente = c.Cliente.IdCliente,
                    Nombre = c.Cliente.Nombre
                },
                Estilista = new EstilistaMiniDto
                {
                    IdEstilista = c.Estilista.IdEstilista,
                    Nombre = c.Estilista.Nombre
                },
                Servicio = new ServicioMiniDto
                {
                    IdServicio = c.Servicio.IdServicio,
                    NombreServicio = c.Servicio.NombreServicio
                }
            });

            return Ok(resultado);
        }

        // ===================== GET POR ID =====================
        [HttpGet("{id}")]
        public async Task<ActionResult<CitaReadDto>> GetCita(int id)
        {
            var c = await _service.ObtenerCita(id);
            if (c == null)
                return NotFound();

            return Ok(new CitaReadDto
            {
                IdCita = c.IdCita,
                Fecha = c.Fecha,
                Estado = c.Estado,

                Cliente = new ClienteMiniDto
                {
                    IdCliente = c.Cliente.IdCliente,
                    Nombre = c.Cliente.Nombre
                },
                Estilista = new EstilistaMiniDto
                {
                    IdEstilista = c.Estilista.IdEstilista,
                    Nombre = c.Estilista.Nombre
                },
                Servicio = new ServicioMiniDto
                {
                    IdServicio = c.Servicio.IdServicio,
                    NombreServicio = c.Servicio.NombreServicio
                }
            });
        }

        // ===================== CREAR =====================
        [HttpPost]
        public async Task<ActionResult<CitaReadDto>> PostCita(CitaCreateDto dto)
        {
            var cita = new Citas
            {
                IdCliente = dto.IdCliente,
                IdEstilista = dto.IdEstilista,
                IdServicio = dto.IdServicio,
                Fecha = dto.Fecha,
                Estado = "Pendiente"
            };

            var creada = await _service.CrearCita(cita);

            return CreatedAtAction(nameof(GetCita), new { id = creada.IdCita }, new CitaReadDto
            {
                IdCita = creada.IdCita,
                Fecha = creada.Fecha,
                Estado = creada.Estado,

                Cliente = new ClienteMiniDto
                {
                    IdCliente = creada.Cliente.IdCliente,
                    Nombre = creada.Cliente.Nombre
                },
                Estilista = new EstilistaMiniDto
                {
                    IdEstilista = creada.Estilista.IdEstilista,
                    Nombre = creada.Estilista.Nombre
                },
                Servicio = new ServicioMiniDto
                {
                    IdServicio = creada.Servicio.IdServicio,
                    NombreServicio = creada.Servicio.NombreServicio
                }
            });
        }

        // ===================== ACTUALIZAR =====================
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, CitaUpdateDto dto)
        {
            var nueva = new Citas
            {
                IdCliente = dto.IdCliente,
                IdEstilista = dto.IdEstilista,
                IdServicio = dto.IdServicio,
                Fecha = dto.Fecha,
                Estado = dto.Estado
            };

            var ok = await _service.ActualizarCita(id, nueva);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        // ===================== CAMBIAR ESTADO =====================
        [HttpPut("estado")]
        public async Task<IActionResult> CambiarEstado([FromBody] CambiarEstadoDto dto)
        {
            var ok = await _service.CambiarEstado(dto.IdCita, dto.NuevoEstado);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        // ===================== ELIMINAR =====================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var ok = await _service.EliminarCita(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }

    public class CambiarEstadoDto
    {
        public int IdCita { get; set; }
        public string NuevoEstado { get; set; } = "";
    }
}
