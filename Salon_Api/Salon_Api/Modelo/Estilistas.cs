namespace Salon_Api.Modelo
{
    public class Estilistas
    {
        public int IdEstilista { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Especialidad { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public bool Estado { get; set; }
        public ICollection<Citas>? Citas { get; set; }
    }
}
