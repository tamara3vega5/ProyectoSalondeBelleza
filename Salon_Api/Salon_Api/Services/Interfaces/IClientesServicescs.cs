using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface IClientesService
    {
        Task<IEnumerable<Clientes>> ObtenerClientes();
        Task<Clientes?> ObtenerClientePorId(int id);
        Task<Clientes> CrearCliente(ClienteCreateDto dto);
        Task<Clientes?> ActualizarCliente(int id, ClienteCreateDto dto);
        Task<bool> EliminarCliente(int id);
    }
}

