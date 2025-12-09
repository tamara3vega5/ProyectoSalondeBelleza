namespace Salon_Api.DTO
{
    public class EstilistaReadDto
    {
        public int IdEstilista { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
    }
}
