using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface IDetalleVentaService
    {
        Task<List<DetalleVenta>> ObtenerDetalles();
        Task<List<DetalleVenta>> ObtenerPorVenta(int idVenta);
        Task<DetalleVenta?> ObtenerDetalle(int id);
        Task<DetalleVenta> CrearDetalle(DetalleVenta detalle);
        Task<bool> EliminarDetalle(int id);
    }
}
