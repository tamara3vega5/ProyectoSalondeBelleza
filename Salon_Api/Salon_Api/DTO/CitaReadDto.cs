namespace Salon_Api.DTO
{
    public class CitaReadDto
    {
        public int IdCita { get; set; }

        public ClienteMiniDto Cliente { get; set; }
        public EstilistaMiniDto Estilista { get; set; }
        public ServicioMiniDto Servicio { get; set; }

        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
