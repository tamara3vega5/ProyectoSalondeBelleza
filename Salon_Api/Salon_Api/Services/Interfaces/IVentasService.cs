using Salon_Api.DTO;

namespace Salon_Api.Services.Interfaces
{
    public interface IVentasService
    {
        Task<VentaDto> CrearVenta(VentaCreateDto dto);
        Task<IEnumerable<VentaDto>> ObtenerVentas();
        Task<VentaDto?> ObtenerVenta(int id);
    }
}
