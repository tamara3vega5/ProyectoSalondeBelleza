namespace Salon_Api.DTO
{
    public class VentaCreateDto
    {
        public int IdCliente { get; set; }

        // Total lo calcularemos automáticamente
        public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
    }
}
