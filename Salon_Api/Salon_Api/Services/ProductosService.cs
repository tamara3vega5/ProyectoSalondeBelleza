using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Services
{
    public class ProductosService : IProductosService
    {
        private readonly ApplicationDbContext _context;

        public ProductosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Productos>> ObtenerProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Productos?> ObtenerProducto(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task<Productos> CrearProducto(Productos p)
        {
            _context.Productos.Add(p);
            await _context.SaveChangesAsync();
            return p;
        }

        public async Task<bool> ActualizarProducto(int id, Productos p)
        {
            var existe = await _context.Productos.FindAsync(id);
            if (existe == null)
                return false;

            existe.NombreProducto = p.NombreProducto;
            existe.Descripcion = p.Descripcion;
            existe.Precio = p.Precio;
            existe.Stock = p.Stock;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarProducto(int id)
        {
            var existe = await _context.Productos.FindAsync(id);
            if (existe == null)
                return false;

            _context.Productos.Remove(existe);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
