using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface ICitasService
    {
        Task<IEnumerable<Citas>> ObtenerCitas();
        Task<Citas?> ObtenerCita(int id);
        Task<Citas> CrearCita(Citas cita);
        Task<bool> ActualizarCita(int id, Citas cita);
        Task<bool> EliminarCita(int id);



    }
}
