using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.DTO;
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

        // Obtener todas las ventas
        public async Task<IEnumerable<VentaDto>> ObtenerVentas()
        {
            try
            {
                var ventas = await _context.Ventas
                    .Include(v => v.DetalleVentas)
                    .ToListAsync();

                return ventas.Select(v => new VentaDto
                {
                    IdVenta = v.IdVenta,
                    IdCliente = v.IdCliente,
                    Total = v.Total,
                    Fecha = v.Fecha,
                    Detalles = v.DetalleVentas.Select(d => new DetalleVentaDto
                    {
                        IdDetalle = d.IdDetalle,
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad,
                        Subtotal = d.Subtotal
                    }).ToList()
                });
            }
            catch
            {
                throw new Exception("Error al obtener las ventas.");
            }
        }

        // Obtener venta por id
        public async Task<VentaDto?> ObtenerVenta(int id)
        {
            try
            {
                var venta = await _context.Ventas
                    .Include(v => v.DetalleVentas)
                    .FirstOrDefaultAsync(v => v.IdVenta == id);

                if (venta == null) return null;

                return new VentaDto
                {
                    IdVenta = venta.IdVenta,
                    IdCliente = venta.IdCliente,
                    Total = venta.Total,
                    Fecha = venta.Fecha,
                    Detalles = venta.DetalleVentas.Select(d => new DetalleVentaDto
                    {
                        IdDetalle = d.IdDetalle,
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad,
                        Subtotal = d.Subtotal
                    }).ToList()
                };
            }
            catch
            {
                throw new Exception("Error al obtener la venta.");
            }
        }

        // Crear venta
        public async Task<VentaDto> CrearVenta(VentaCreateDto dto)
        {
            try
            {
                if (dto.Detalles == null || !dto.Detalles.Any())
                    throw new Exception("La venta debe tener al menos un producto.");

                var clienteExiste = await _context.Clientes.AnyAsync(c => c.IdCliente == dto.IdCliente);
                if (!clienteExiste)
                    throw new Exception($"El cliente con Id {dto.IdCliente} no existe.");

                var venta = new Ventas
                {
                    IdCliente = dto.IdCliente,
                    Fecha = DateTime.Now
                };

                await _context.Ventas.AddAsync(venta);
                await _context.SaveChangesAsync();  // Genera IdVenta

                decimal total = 0;

                foreach (var item in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(item.IdProducto);
                    if (producto == null) throw new Exception($"Producto {item.IdProducto} no existe");
                    if (producto.Stock < item.Cantidad) throw new Exception($"Stock insuficiente para {producto.NombreProducto}");

                    producto.Stock -= item.Cantidad;
                    _context.Productos.Update(producto);

                    var subtotal = producto.Precio * item.Cantidad;
                    total += subtotal;

                    var detalle = new DetalleVenta
                    {
                        IdVenta = venta.IdVenta,
                        IdProducto = producto.IdProducto,
                        Cantidad = item.Cantidad,
                        Subtotal = subtotal
                    };

                    await _context.DetalleVentas.AddAsync(detalle);
                }

                venta.Total = total;
                _context.Ventas.Update(venta);

                await _context.SaveChangesAsync();

                return await ObtenerVenta(venta.IdVenta) ?? throw new Exception("Error al generar la venta.");
            }
            catch
            {
                throw new Exception("Error al registrar la venta.");
            }
        }
    }
}
