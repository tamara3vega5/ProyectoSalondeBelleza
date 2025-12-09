namespace Salon_Api.DTO
{
    public class ServicioReadDto
    {
        public int IdServicio { get; set; }
        public string NombreServicio { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int DuracionMin { get; set; }
        public decimal Precio { get; set; }
    }
}
