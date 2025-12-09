using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Services
{
    public class ServiciosService : IServiciosService
    {
        private readonly ApplicationDbContext _context;

        public ServiciosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Servicios>> ObtenerServicios()
        {
            return await _context.Servicios.ToListAsync();
        }

        public async Task<Servicios?> ObtenerServicio(int id)
        {
            return await _context.Servicios.FindAsync(id);
        }

        public async Task<Servicios> CrearServicio(Servicios s)
        {
            _context.Servicios.Add(s);
            await _context.SaveChangesAsync();
            return s;
        }

        public async Task<bool> ActualizarServicio(int id, Servicios s)
        {
            var existe = await _context.Servicios.FindAsync(id);
            if (existe == null)
                return false;

            existe.NombreServicio = s.NombreServicio;
            existe.Descripcion = s.Descripcion;
            existe.DuracionMin = s.DuracionMin;
            existe.Precio = s.Precio;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarServicio(int id)
        {
            var existe = await _context.Servicios.FindAsync(id);
            if (existe == null)
                return false;

            _context.Servicios.Remove(existe);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
