namespace Salon_Api.Modelo
{
    public class DetalleVenta
    {
        public int IdDetalle { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }

        public Ventas? Venta { get; set; }
        public Productos? Producto { get; set; }
    }
}

