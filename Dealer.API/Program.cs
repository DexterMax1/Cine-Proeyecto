using Microsoft.EntityFrameworkCore;
using Dealer.Aplication.Interfaces;
using Dealer.Infrastructure.Data;
using Dealer.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios básicos y controladores
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configuración de la conexión a SQL Server con Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de servicios para la inyección de dependencias
builder.Services.AddScoped<ICinemaService, CinemaService>();

var app = builder.Build();

// Inicialización y población automática de la base de datos (Seed)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

// Configuración del entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware de seguridad y enrutamiento
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();