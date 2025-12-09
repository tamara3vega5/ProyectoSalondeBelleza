using Salon_Api.Modelo;

namespace Salon_Api.Services.Interfaces
{
    public interface IProductosService
    {
        Task<List<Productos>> ObtenerProductos();
        Task<Productos?> ObtenerProducto(int id);
        Task<Productos> CrearProducto(Productos p);
        Task<bool> ActualizarProducto(int id, Productos p);
        Task<bool> EliminarProducto(int id);
    }
}
