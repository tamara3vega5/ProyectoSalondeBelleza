using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Services;
using Salon_Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Entity Framework configuración con SQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de Servicios
builder.Services.AddScoped<IVentasService, VentasService>();
//builder.Services.AddScoped<IDetalleVentasService, DetalleVentasService>();
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<ICitasService, CitasService>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Controladores
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para frontend
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

// Middleware
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
