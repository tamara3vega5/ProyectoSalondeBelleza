using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;

// Interfaces
using Salon_Api.Services.Interfaces;

// Servicios
using Salon_Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ================================================================
//   1. CONFIGURAR SQL SERVER (CONEXIÓN A LA BASE DE DATOS)
// ================================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ================================================================
//   2. REGISTRO DE TODOS LOS SERVICES (DEPENDENCY INJECTION)
// ================================================================

// 🔐 Autenticación
builder.Services.AddScoped<IAuthService, AuthService>();

// 🧍 Clientes
builder.Services.AddScoped<IClientesService, ClientesService>();

// 💇 Estilistas
builder.Services.AddScoped<IEstilistasService, EstilistasService>();

// 💅 Servicios del salón
builder.Services.AddScoped<IServiciosService, ServiciosService>();

// 🛍 Productos
builder.Services.AddScoped<IProductosService, ProductosService>();

// 📆 Citas
builder.Services.AddScoped<ICitasService, CitasService>();

// 🧾 Ventas
builder.Services.AddScoped<IVentasService, VentasService>();

// 🧾 Detalle de Venta
builder.Services.AddScoped<IDetalleVentaService, DetalleVentaService>();



// ================================================================
//   3. CONTROLADORES
// ================================================================
builder.Services.AddControllers();


// ================================================================
//   4. SWAGGER (Documentación API)
// ================================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ================================================================
//   5. CORS — PERMITIR ACCESO DESDE EL FRONTEND
// ================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();


// ================================================================
//   6. MIDDLEWARE
// ================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
