namespace Salon_Api.Modelo
{
    public class Citas
    {
        public int IdCita { get; set; }
        public int IdCliente { get; set; }
        public int IdEstilista { get; set; }
        public int IdServicio { get; set; }
        public DateTime Fecha { get; set; } 
        public string? Estado { get; set; } = "Confirmado"; //Aquí tmb puede ser cancelada,confirmada o completada
        public Clientes? Cliente { get; set; }
        public Estilistas? Estilista { get; set; }
        public Servicios? Servicio { get; set; }
    }
}
