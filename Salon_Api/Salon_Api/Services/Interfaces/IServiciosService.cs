using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface IServiciosService
    {
        Task<List<Servicios>> ObtenerServicios();
        Task<Servicios?> ObtenerServicio(int id);
        Task<Servicios> CrearServicio(Servicios s);
        Task<bool> ActualizarServicio(int id, Servicios s);
        Task<bool> EliminarServicio(int id);
    }
}
