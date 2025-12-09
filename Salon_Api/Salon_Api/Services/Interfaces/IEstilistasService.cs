using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface IEstilistasService
    {
        Task<List<Estilistas>> ObtenerEstilistas();
        Task<Estilistas?> ObtenerEstilista(int id);
        Task<Estilistas> CrearEstilista(Estilistas e);
        Task<bool> ActualizarEstilista(int id, Estilistas e);
        Task<bool> EliminarEstilista(int id);
    }
}
