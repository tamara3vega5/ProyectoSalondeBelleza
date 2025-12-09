namespace Salon_Api.DTO
{
    public class VentaCreateDto
    {
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
    }
}
