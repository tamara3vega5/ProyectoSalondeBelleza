using Microsoft.EntityFrameworkCore;
using Salon_Api.Modelo;

namespace Salon_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public required DbSet<Citas> Citas { get; set; }
        public required DbSet<Clientes> Clientes { get; set; }
        public required DbSet<DetalleVenta> DetalleVentas { get; set; } // nombre DbSet puede quedar plural
        public required DbSet<Estilistas> Estilistas { get; set; }
        public required DbSet<Productos> Productos { get; set; }
        public required DbSet<Servicios> Servicios { get; set; }
        public required DbSet<Ventas> Ventas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Asignar nombres exactos de tablas ---
            modelBuilder.Entity<DetalleVenta>().ToTable("DetalleVenta"); // <-- aquí se corrige el nombre

            modelBuilder.Entity<Citas>().HasKey(c => c.IdCita);
            modelBuilder.Entity<Clientes>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<DetalleVenta>().HasKey(d => d.IdDetalle);
            modelBuilder.Entity<Estilistas>().HasKey(e => e.IdEstilista);
            modelBuilder.Entity<Productos>().HasKey(p => p.IdProducto);
            modelBuilder.Entity<Servicios>().HasKey(s => s.IdServicio);
            modelBuilder.Entity<Ventas>().HasKey(v => v.IdVenta);

            ConfigurarRelaciones(modelBuilder);
        }

        private void ConfigurarRelaciones(ModelBuilder modelBuilder)
        {
            // --- Citas ---
            modelBuilder.Entity<Citas>()
                .HasOne(c => c.Cliente)
                .WithMany(cli => cli.Citas)
                .HasForeignKey(c => c.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Citas>()
                .HasOne(c => c.Estilista)
                .WithMany(e => e.Citas)
                .HasForeignKey(c => c.IdEstilista)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Citas>()
                .HasOne(c => c.Servicio)
                .WithMany(s => s.Citas)
                .HasForeignKey(c => c.IdServicio)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Ventas ---
            modelBuilder.Entity<Ventas>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            // --- DetalleVenta -> Venta ---
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Venta)
                .WithMany(v => v.DetalleVentas)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.Cascade);

            // --- DetalleVenta -> Producto ---
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetalleVentas)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
