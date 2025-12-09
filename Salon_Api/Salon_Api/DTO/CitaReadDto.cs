namespace Salon_Api.DTO
{
    public class CitaReadDto
    {
        public int IdCita { get; set; }
        public string Cliente { get; set; } = "";
        public string Estilista { get; set; } = "";
        public string Servicio { get; set; } = ""; // Aquí usaremos NombreServicio
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "";
    }
}
