namespace Salon_Api.DTO
{
    public class CitaUpdateDto
    {
        public int IdCliente { get; set; }
        public int IdEstilista { get; set; }
        public int IdServicio { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "Confirmado";

    }
}
