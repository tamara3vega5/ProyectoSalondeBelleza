using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.DTO;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;
using BCrypt.Net;

namespace Salon_Api.Services
{
    public class ClientesService : IClientesService
    {
        private readonly ApplicationDbContext _context;

        public ClientesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Clientes>> ObtenerClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Clientes?> ObtenerClientePorId(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        // ✅ Crear Cliente con DTO
        public async Task<Clientes> CrearCliente(ClienteCreateDto dto)
        {
            var nuevoCliente = new Clientes
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                FechaRegistro = dto.FechaRegistro,

                // 👇 Hashear la contraseña ANTES de guardar
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            return nuevoCliente;
        }


        // ✅ Actualizar Cliente con DTO
        public async Task<Clientes?> ActualizarCliente(int id, ClienteCreateDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return null;

            cliente.Nombre = dto.Nombre;
            cliente.Telefono = dto.Telefono;
            cliente.Correo = dto.Correo;
            cliente.FechaRegistro = dto.FechaRegistro;

            await _context.SaveChangesAsync();

            return cliente;
        }


        public async Task<bool> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
