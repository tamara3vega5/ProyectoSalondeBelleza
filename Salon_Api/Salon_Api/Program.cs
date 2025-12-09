using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Services;
using Salon_Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios de dominio
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICitasService, CitasService>();
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IVentasService, VentasService>();
builder.Services.AddScoped<IServiciosService, ServiciosService>();
builder.Services.AddScoped<IEstilistasService, EstilistasService>();
builder.Services.AddScoped<IProductosService, ProductosService>();
builder.Services.AddScoped<IDetalleVentaService, DetalleVentaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS abierto para tu frontend
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p => p
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();
app.Run();
