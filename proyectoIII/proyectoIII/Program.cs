using proyectoIII.data;
using Microsoft.EntityFrameworkCore;
using proyectoIII.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Base de datos en memoria
builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseInMemoryDatabase("EducacionDB"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AplicationDbContext>();

    context.Usuarios.Add(new proyectoIII.Models.Usuario
    {
        Nombre = "Deymar",
        Email = "admin@prueba.com",
        Password = "123",
        Rol = "Admin"
    });
    context.Usuarios.Add(new proyectoIII.Models.Usuario
    {
        Nombre = "Estudiante",
        Email = "estudiante@prueba.com",
        Password = "123",
        Rol = "Estudiante"
    });

    context.SaveChanges();
}

app.Run();