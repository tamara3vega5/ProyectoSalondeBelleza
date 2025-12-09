namespace Salon_Api.DTO
{
    public class VentaReadDto
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetalleVentaDto>? Detalles { get; set; }
    }
}