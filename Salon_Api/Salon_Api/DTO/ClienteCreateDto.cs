namespace Salon_Api.DTO
{
    public class ClienteCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string Password { get; set; } = null!; // ?? NUEVO
        public DateTime FechaRegistro { get; set; }
    }
}
