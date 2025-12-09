namespace Salon_Api.Modelo
{
    public class Clientes
    {
        public int IdCliente { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        // Password almacenado en HASH BCrypt
        public string PasswordHash { get; set; } = null!;

        // Rol del cliente (user / admin)
        public string Rol { get; set; } = "user";

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relaciones
        public ICollection<Ventas>? Ventas { get; set; }
        public ICollection<Citas>? Citas { get; set; }
    }
}

