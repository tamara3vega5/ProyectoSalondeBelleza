namespace Salon_Api.Modelo
{
    public class Productos
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = null!;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Descripcion { get; set; }
        public ICollection<DetalleVenta>? DetalleVentas { get; set; }
    }
        
}
