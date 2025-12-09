using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Services
{
    public class DetalleVentaService : IDetalleVentaService
    {
        private readonly ApplicationDbContext _context;

        public DetalleVentaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DetalleVenta>> ObtenerDetalles()
        {
            return await _context.DetalleVentas
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .ToListAsync();
        }

        public async Task<List<DetalleVenta>> ObtenerPorVenta(int idVenta)
        {
            return await _context.DetalleVentas
                .Where(d => d.IdVenta == idVenta)
                .Include(d => d.Producto)
                .ToListAsync();
        }

        public async Task<DetalleVenta?> ObtenerDetalle(int id)
        {
            return await _context.DetalleVentas
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .FirstOrDefaultAsync(d => d.IdDetalle == id);
        }

        public async Task<DetalleVenta> CrearDetalle(DetalleVenta detalle)
        {
            _context.DetalleVentas.Add(detalle);
            await _context.SaveChangesAsync();
            return detalle;
        }

        public async Task<bool> EliminarDetalle(int id)
        {
            var detalle = await _context.DetalleVentas.FindAsync(id);
            if (detalle == null)
                return false;

            _context.DetalleVentas.Remove(detalle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
