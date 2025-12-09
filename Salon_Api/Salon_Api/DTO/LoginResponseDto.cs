namespace Salon_Api.DTO
{
    public class LoginResponseDto
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Rol { get; set; } = "cliente";
        public string Token { get; set; } = string.Empty;
    }
}
