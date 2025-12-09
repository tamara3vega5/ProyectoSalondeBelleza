using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Services
{
    public class VentasService : IVentasService
    {
        private readonly ApplicationDbContext _context;

        public VentasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ventas>> ObtenerVentas()
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Producto)
                .ToListAsync();
        }

        public async Task<Ventas?> ObtenerVenta(int id)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.IdVenta == id);
        }

        public async Task<Ventas> CrearVenta(Ventas venta, List<DetalleVenta> detalles)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                foreach (var d in detalles)
                {
                    d.IdVenta = venta.IdVenta;
                    _context.DetalleVentas.Add(d);
                }

                await _context.SaveChangesAsync();
                await trans.CommitAsync();

                return await ObtenerVenta(venta.IdVenta);
            }
            catch
            {
                await trans.RollbackAsync();
                throw new Exception("Error al registrar la venta.");
            }
        }

        public async Task<bool> EliminarVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return false;

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

