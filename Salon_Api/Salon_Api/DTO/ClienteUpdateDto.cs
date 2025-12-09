namespace Salon_Api.DTO
{
    public class ClienteUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }

        public string? NuevaContrasena { get; set; } // opcional
        public string? Rol { get; set; }             // opcional
    }
}
