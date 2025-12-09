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
            // ===========================================
            // VALIDACIONES BÁSICAS
            // ===========================================
            if (string.IsNullOrWhiteSpace(dto.Correo) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return null;

            string correoNormalizado = dto.Correo.Trim().ToLower();

            // ===========================================
            // BUSCAR USUARIO
            // ===========================================
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Correo.ToLower() == correoNormalizado);

            if (cliente == null)
                return null;

            // ===========================================
            // SI EL PASSWORD FUE GUARDADO SIN HASH (COMPATIBILIDAD)
            // ===========================================
            if (!cliente.PasswordHash.StartsWith("$2"))
            {
                // Contraseña sin hash → solo coincide si es EXACTA
                if (cliente.PasswordHash == dto.Password)
                    return cliente;

                return null;
            }

            // ===========================================
            // VALIDAR CONTRASEÑA HASHED BCRYPT
            // ===========================================
            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, cliente.PasswordHash);

            if (!valid)
                return null;

            // ===========================================
            // GARANTIZAR QUE EL ROL NUNCA SEA NULL
            // ===========================================
            if (string.IsNullOrWhiteSpace(cliente.Rol))
                cliente.Rol = "cliente";

            return cliente;
        }
    }
}
