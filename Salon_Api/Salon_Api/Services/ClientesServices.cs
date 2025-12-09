using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Services
{
    public class ClientesService : IClientesService
    {
        private readonly ApplicationDbContext _context;

        public ClientesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // Obtener todos los clientes
        // ============================================================
        public async Task<List<Clientes>> ObtenerClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // ============================================================
        // Obtener cliente por ID
        // ============================================================
        public async Task<Clientes?> ObtenerCliente(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        // ============================================================
        // Crear cliente
        // ============================================================
        public async Task<Clientes> CrearCliente(Clientes c)
        {
            // Normalizar correo
            c.Correo = c.Correo?.Trim().ToLower();

            // Validar duplicación
            bool existeCorreo = await _context.Clientes.AnyAsync(x => x.Correo == c.Correo);
            if (existeCorreo)
                throw new Exception("El correo ya está registrado.");

            _context.Clientes.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }

        // ============================================================
        // Actualizar cliente
        // ============================================================
        public async Task<bool> ActualizarCliente(int id, Clientes c)
        {
            var existe = await _context.Clientes.FindAsync(id);
            if (existe == null)
                return false;

            c.Correo = c.Correo?.Trim().ToLower();

            // ¿El nuevo correo está en uso por OTRO cliente?
            if (!string.IsNullOrWhiteSpace(c.Correo) && c.Correo != existe.Correo)
            {
                bool correoEnUso = await _context.Clientes.AnyAsync(x => x.Correo == c.Correo);
                if (correoEnUso)
                    throw new Exception("El correo ya está siendo usado por otro usuario.");
            }

            // Actualizar campos
            existe.Nombre = c.Nombre;
            existe.Telefono = c.Telefono;
            existe.Correo = c.Correo;

            // actualizar rol si viene
            if (!string.IsNullOrWhiteSpace(c.Rol))
                existe.Rol = c.Rol.Trim().ToLower();

            // actualizar contraseña si viene hash
            if (!string.IsNullOrWhiteSpace(c.PasswordHash))
                existe.PasswordHash = c.PasswordHash;

            await _context.SaveChangesAsync();
            return true;
        }

        // ============================================================
        // Eliminar cliente
        // ============================================================
        public async Task<bool> EliminarCliente(int id)
        {
            var existe = await _context.Clientes
                .Include(c => c.Ventas)
                .Include(c => c.Citas)
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (existe == null)
                return false;

            // Evitar romper relaciones
            if (existe.Ventas?.Any() == true)
                throw new Exception("No se puede eliminar un cliente con ventas registradas.");

            if (existe.Citas?.Any() == true)
                throw new Exception("No se puede eliminar un cliente con citas activas.");

            _context.Clientes.Remove(existe);
            await _context.SaveChangesAsync();

            return true;
        }

        // ============================================================
        // Cambiar contraseña
        // ============================================================
        public async Task<bool> CambiarPassword(int id, string newHash)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.PasswordHash = newHash;
            await _context.SaveChangesAsync();
            return true;
        }

        // ============================================================
        // Cambiar rol (admin / user)
        // ============================================================
        public async Task<bool> CambiarRol(int id, string nuevoRol)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.Rol = nuevoRol.Trim().ToLower();
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


