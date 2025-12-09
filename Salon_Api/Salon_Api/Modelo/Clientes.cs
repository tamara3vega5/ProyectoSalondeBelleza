namespace Salon_Api.Modelo
{
    public class Clientes
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }

        public string PasswordHash { get; set; } = null!; 

        public DateTime FechaRegistro { get; set; }
        public ICollection<Ventas>? Ventas { get; set; }
        public ICollection<Citas>? Citas { get; set; }
    }
}
