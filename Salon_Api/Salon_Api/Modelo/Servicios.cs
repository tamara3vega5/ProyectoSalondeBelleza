namespace Salon_Api.Modelo
{
    public class Servicios
    {
        public int IdServicio { get; set; }
        public string NombreServicio { get; set; } = null!;
        public decimal Precio { get; set; }
        public int DuracionMin { get; set; }
        public string? Descripcion { get; set; }
        public ICollection<Citas>? Citas { get; set; }
    }
}
