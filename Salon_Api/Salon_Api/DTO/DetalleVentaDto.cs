namespace Salon_Api.DTO
{
    public class DetalleVentaDto
    {
        public int IdDetalle { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
