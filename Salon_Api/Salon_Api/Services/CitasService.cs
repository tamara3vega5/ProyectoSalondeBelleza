using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Services
{
    public class CitasService : ICitasService
    {
        private readonly ApplicationDbContext _context;

        public CitasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Citas>> ObtenerCitas()
        {
            try
            {
                return await _context.Citas
                    .Include(c => c.Cliente)
                    .Include(c => c.Estilista)
                    .Include(c => c.Servicio)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las citas: " + ex.Message);
            }
        }

        public async Task<Citas?> ObtenerCita(int id)
        {
            try
            {
                return await _context.Citas
                    .Include(c => c.Cliente)
                    .Include(c => c.Estilista)
                    .Include(c => c.Servicio)
                    .FirstOrDefaultAsync(c => c.IdCita == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la cita: " + ex.Message);
            }
        }
        public async Task<Citas> CrearCita(Citas cita)
        {
            try
            {
                // Validación: estilista ocupado
                bool estilistaOcupado = await _context.Citas
                    .AnyAsync(c =>
                        c.IdEstilista == cita.IdEstilista &&
                        c.Fecha.Date == cita.Fecha.Date &&
                        c.Fecha.Hour == cita.Fecha.Hour &&
                        c.Fecha.Minute == cita.Fecha.Minute
                    );

                if (estilistaOcupado)
                    throw new Exception("El estilista ya tiene una cita en esa fecha y hora.");

                // Validación: cliente ya tiene cita a la misma hora
                bool clienteOcupado = await _context.Citas
                    .AnyAsync(c =>
                        c.IdCliente == cita.IdCliente &&
                        c.Fecha.Date == cita.Fecha.Date &&
                        c.Fecha.Hour == cita.Fecha.Hour &&
                        c.Fecha.Minute == cita.Fecha.Minute
                    );

                if (clienteOcupado)
                    throw new Exception("El cliente ya tiene una cita en esa fecha y hora.");

                // Guardar cita
                _context.Citas.Add(cita);
                await _context.SaveChangesAsync();
                return cita;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la cita: " + ex.Message);
            }
        }


        // ==========================================================
        //                 ACTUALIZAR CITA (PUT)
        // ==========================================================
        public async Task<bool> ActualizarCita(int id, Citas cita)
        {
            try
            {
                var existe = await _context.Citas.FindAsync(id);
                if (existe == null) return false;

                // Validación: Estilista ocupado (excepto su propia cita)
                bool estilistaOcupado = await _context.Citas
                    .AnyAsync(c =>
                        c.IdEstilista == cita.IdEstilista &&
                        c.Fecha == cita.Fecha &&
                        c.IdCita != id
                    );

                if (estilistaOcupado)
                    throw new Exception("El estilista ya tiene una cita en esa fecha y hora.");

                // Validación: Cliente ocupado (excepto su propia cita)
                bool clienteOcupado = await _context.Citas
                    .AnyAsync(c =>
                        c.IdCliente == cita.IdCliente &&
                        c.Fecha == cita.Fecha &&
                        c.IdCita != id
                    );

                if (clienteOcupado)
                    throw new Exception("El cliente ya tiene una cita en esa fecha y hora.");

                // Actualizar datos
                existe.IdCliente = cita.IdCliente;
                existe.IdEstilista = cita.IdEstilista;
                existe.IdServicio = cita.IdServicio;
                existe.Fecha = cita.Fecha;
                existe.Estado = cita.Estado;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // ==========================================================
        //                 ELIMINAR CITA
        // ==========================================================
        public async Task<bool> EliminarCita(int id)
        {
            try
            {
                var cita = await _context.Citas.FindAsync(id);
                if (cita == null) return false;

                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la cita: " + ex.Message);
            }
        }
    }
}


