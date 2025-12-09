namespace Salon_Api.Modelo
{
    public class Ventas
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }

        public Clientes? Cliente { get; set; }
        public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
    }
}
