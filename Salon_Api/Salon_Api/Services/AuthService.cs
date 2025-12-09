using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.DTO;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Clientes?> Login(LoginDto dto)
        {
            // Buscar cliente por correo
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Correo == dto.Correo);

            if (cliente == null)
                return null;

            // Validar contraseña
            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, cliente.PasswordHash);

            if (!valid)
                return null;

            return cliente; // El login es correcto
        }
    }
}
