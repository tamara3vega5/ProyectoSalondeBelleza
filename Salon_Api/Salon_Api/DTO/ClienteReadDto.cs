namespace Salon_Api.DTO
{
    public class ClienteReadDto
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string Rol { get; set; } = "cliente";
    }
}
