using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Services
{
    public class EstilistasService : IEstilistasService
    {
        private readonly ApplicationDbContext _context;

        public EstilistasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Estilistas>> ObtenerEstilistas()
        {
            return await _context.Estilistas.ToListAsync();
        }

        public async Task<Estilistas?> ObtenerEstilista(int id)
        {
            return await _context.Estilistas.FindAsync(id);
        }

        public async Task<Estilistas> CrearEstilista(Estilistas e)
        {
            _context.Estilistas.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> ActualizarEstilista(int id, Estilistas e)
        {
            var existe = await _context.Estilistas.FindAsync(id);
            if (existe == null)
                return false;

            existe.Nombre = e.Nombre;
            existe.Especialidad = e.Especialidad;
            existe.Telefono = e.Telefono;
            existe.Correo = e.Correo;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarEstilista(int id)
        {
            var existe = await _context.Estilistas.FindAsync(id);
            if (existe == null)
                return false;

            _context.Estilistas.Remove(existe);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
wai