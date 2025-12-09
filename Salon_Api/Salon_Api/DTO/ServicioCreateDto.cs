namespace Salon_Api.DTO
{
    public class ServicioCreateDto
    {
        public string NombreServicio { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int DuracionMin { get; set; }
        public decimal Precio { get; set; }
    }
}
