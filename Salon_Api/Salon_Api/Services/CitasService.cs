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

        // Obtener TODAS las citas con relaciones
        public async Task<List<Citas>> ObtenerCitas()
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Estilista)
                .Include(c => c.Servicio)
                .ToListAsync();
        }

        // Obtener una sola cita
        public async Task<Citas?> ObtenerCita(int id)
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Estilista)
                .Include(c => c.Servicio)
                .FirstOrDefaultAsync(c => c.IdCita == id);
        }

        // Crear cita
        public async Task<Citas> CrearCita(Citas cita)
        {
            // Estilista ocupado
            bool estilistaOcupado = await _context.Citas.AnyAsync(c =>
                c.IdEstilista == cita.IdEstilista &&
                c.Fecha == cita.Fecha
            );

            if (estilistaOcupado)
                throw new Exception("El estilista ya tiene una cita en ese horario.");

            // Cliente ocupado
            bool clienteOcupado = await _context.Citas.AnyAsync(c =>
                c.IdCliente == cita.IdCliente &&
                c.Fecha == cita.Fecha
            );

            if (clienteOcupado)
                throw new Exception("El cliente ya posee otra cita en ese horario.");

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return await ObtenerCita(cita.IdCita);
        }

        // Actualizar cita
        public async Task<bool> ActualizarCita(int id, Citas nueva)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
                return false;

            // Validación estilista
            bool estilistaOcupado = await _context.Citas.AnyAsync(c =>
                c.IdEstilista == nueva.IdEstilista &&
                c.Fecha == nueva.Fecha &&
                c.IdCita != id
            );

            if (estilistaOcupado)
                throw new Exception("El estilista ya tiene una cita en ese horario.");

            // Validación cliente
            bool clienteOcupado = await _context.Citas.AnyAsync(c =>
                c.IdCliente == nueva.IdCliente &&
                c.Fecha == nueva.Fecha &&
                c.IdCita != id
            );

            if (clienteOcupado)
                throw new Exception("El cliente ya tiene una cita en ese horario.");

            cita.IdCliente = nueva.IdCliente;
            cita.IdEstilista = nueva.IdEstilista;
            cita.IdServicio = nueva.IdServicio;
            cita.Fecha = nueva.Fecha;
            cita.Estado = nueva.Estado;

            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar cita
        public async Task<bool> EliminarCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
                return false;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return true;
        }

        // Cambiar estado
        public async Task<bool> CambiarEstado(int idCita, string nuevoEstado)
        {
            var cita = await _context.Citas.FindAsync(idCita);
            if (cita == null)
                return false;

            cita.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


