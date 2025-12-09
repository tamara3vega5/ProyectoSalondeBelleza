using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface IClientesService
    {
        Task<List<Clientes>> ObtenerClientes();
        Task<Clientes?> ObtenerCliente(int id);
        Task<Clientes> CrearCliente(Clientes c);
        Task<bool> ActualizarCliente(int id, Clientes c);
        Task<bool> EliminarCliente(int id);

        // Nuevos métodos agregados
        Task<bool> CambiarPassword(int id, string newHash);
        Task<bool> CambiarRol(int id, string nuevoRol);
    }
}

