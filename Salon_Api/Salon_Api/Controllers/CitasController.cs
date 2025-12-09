using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ICitasService _service;

        public CitasController(ICitasService service)
        {
            _service = service;
        }

        // GET: api/Citas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaReadDto>>> GetCitas()
        {
            try
            {
                var citas = await _service.ObtenerCitas();

                var resultado = citas.Select(c => new CitaReadDto
                {
                    IdCita = c.IdCita,
                    Cliente = c.Cliente?.Nombre ?? "",
                    Estilista = c.Estilista?.Nombre ?? "",
                    Servicio = c.Servicio?.NombreServicio ?? "",
                    Fecha = c.Fecha,
                    Estado = c.Estado
                });

                return Ok(resultado);
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al obtener las citas. Intenta nuevamente." });
            }
        }

        // GET: api/Citas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CitaReadDto>> GetCita(int id)
        {
            try
            {
                var c = await _service.ObtenerCita(id);
                if (c == null)
                    return NotFound(new { mensaje = "Cita no encontrada" });

                var dto = new CitaReadDto
                {
                    IdCita = c.IdCita,
                    Cliente = c.Cliente?.Nombre ?? "",
                    Estilista = c.Estilista?.Nombre ?? "",
                    Servicio = c.Servicio?.NombreServicio ?? "",
                    Fecha = c.Fecha,
                    Estado = c.Estado
                };

                return Ok(dto);
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al obtener la cita. Intenta nuevamente." });
            }
        }

        // POST: api/Citas
        [HttpPost]
        public async Task<ActionResult<CitaReadDto>> PostCita([FromBody] CitaCreateDto dto)
        {
            try
            {
                // 1. Validación: fecha pasada
                if (dto.Fecha < DateTime.Now)
                    return BadRequest(new { mensaje = "No se puede crear una cita en una fecha/hora pasada." });

                // 2. Validación: horario del salón (ej: 9:00 - 18:00)
                TimeSpan horaApertura = new TimeSpan(9, 0, 0);
                TimeSpan horaCierre = new TimeSpan(18, 0, 0);

                if (dto.Fecha.TimeOfDay < horaApertura || dto.Fecha.TimeOfDay > horaCierre)
                    return BadRequest(new { mensaje = "La cita debe estar dentro del horario del salón (09:00 a 18:00)." });

                var citasExistentes = await _service.ObtenerCitas();

                // 3. Validación: estilista ocupado
                bool estilistaOcupado = citasExistentes.Any(c =>
                    c.IdEstilista == dto.IdEstilista &&
                    c.Fecha.Date == dto.Fecha.Date &&
                    c.Fecha.Hour == dto.Fecha.Hour &&
                    c.Fecha.Minute == dto.Fecha.Minute
                );

                if (estilistaOcupado)
                    return BadRequest(new { mensaje = "El estilista ya tiene una cita en esa fecha y hora." });

                // 4. Validación: cliente ocupado
                bool clienteOcupado = citasExistentes.Any(c =>
                    c.IdCliente == dto.IdCliente &&
                    c.Fecha.Date == dto.Fecha.Date &&
                    c.Fecha.Hour == dto.Fecha.Hour &&
                    c.Fecha.Minute == dto.Fecha.Minute
                );

                if (clienteOcupado)
                    return BadRequest(new { mensaje = "El cliente ya tiene una cita en esa fecha y hora." });

                // Crear la cita
                var cita = new Citas
                {
                    IdCliente = dto.IdCliente,
                    IdEstilista = dto.IdEstilista,
                    IdServicio = dto.IdServicio,
                    Fecha = dto.Fecha,
                    Estado = "Confirmado"
                };

                var nueva = await _service.CrearCita(cita);

                var readDto = new CitaReadDto
                {
                    IdCita = nueva.IdCita,
                    Cliente = nueva.Cliente?.Nombre ?? "",
                    Estilista = nueva.Estilista?.Nombre ?? "",
                    Servicio = nueva.Servicio?.NombreServicio ?? "",
                    Fecha = nueva.Fecha,
                    Estado = nueva.Estado
                };

                return CreatedAtAction(nameof(GetCita), new { id = nueva.IdCita }, readDto);
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al crear la cita. Intenta nuevamente." });
            }
        }

        // PUT: api/Citas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, [FromBody] CitaUpdateDto dto)
        {
            try
            {
                // Validación: fecha pasada
                if (dto.Fecha < DateTime.Now)
                    return BadRequest(new { mensaje = "No se puede asignar una fecha/hora pasada a la cita." });

                // Validación: horario del salón
                TimeSpan apertura = new TimeSpan(9, 0, 0);
                TimeSpan cierre = new TimeSpan(18, 0, 0);

                if (dto.Fecha.TimeOfDay < apertura || dto.Fecha.TimeOfDay > cierre)
                    return BadRequest(new { mensaje = "La cita debe estar dentro del horario del salón (09:00 a 18:00)." });

                var citas = await _service.ObtenerCitas();

                // Validar estilista
                bool estilistaOcupado = citas.Any(c =>
                    c.IdCita != id &&
                    c.IdEstilista == dto.IdEstilista &&
                    c.Fecha == dto.Fecha
                );

                if (estilistaOcupado)
                    return BadRequest(new { mensaje = "El estilista ya tiene una cita programada para esa fecha y hora." });

                // Validar cliente
                bool clienteOcupado = citas.Any(c =>
                    c.IdCita != id &&
                    c.IdCliente == dto.IdCliente &&
                    c.Fecha == dto.Fecha
                );

                if (clienteOcupado)
                    return BadRequest(new { mensaje = "El cliente ya tiene otra cita programada en la misma fecha y hora." });

                // Actualizar cita
                var cita = new Citas
                {
                    IdCliente = dto.IdCliente,
                    IdEstilista = dto.IdEstilista,
                    IdServicio = dto.IdServicio,
                    Fecha = dto.Fecha,
                    Estado = dto.Estado
                };

                var ok = await _service.ActualizarCita(id, cita);

                if (!ok)
                    return NotFound(new { mensaje = "Cita no encontrada" });

                return NoContent();
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al actualizar la cita. Intenta nuevamente." });
            }
        }

        // DELETE: api/Citas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            try
            {
                var ok = await _service.EliminarCita(id);

                if (!ok)
                    return NotFound(new { mensaje = "Cita no encontrada" });

                return NoContent();
            }
            catch
            {
                return BadRequest(new { mensaje = "Ocurrió un error al eliminar la cita. Intenta nuevamente." });
            }
        }
    }
}
