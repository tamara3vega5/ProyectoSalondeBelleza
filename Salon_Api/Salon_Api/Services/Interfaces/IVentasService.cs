using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface IVentasService
    {
        Task<List<Ventas>> ObtenerVentas();
        Task<Ventas?> ObtenerVenta(int id);
        Task<Ventas> CrearVenta(Ventas venta, List<DetalleVenta> detalles);
        Task<bool> EliminarVenta(int id);
    }
}
