using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface ICitasService
    {
        Task<List<Citas>> ObtenerCitas();
        Task<Citas?> ObtenerCita(int id);
        Task<Citas> CrearCita(Citas cita);
        Task<bool> ActualizarCita(int id, Citas citaActualizada);
        Task<bool> EliminarCita(int id);
        Task<bool> CambiarEstado(int idCita, string nuevoEstado);
    }
}
