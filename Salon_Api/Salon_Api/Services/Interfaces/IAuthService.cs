using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Clientes?> Login(LoginDto dto);
    }
}
