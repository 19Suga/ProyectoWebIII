using proyectoIII.data;
using Microsoft.EntityFrameworkCore;
using proyectoIII.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Base de datos en memoria
builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseInMemoryDatabase("EducacionDB"));
;

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

// Datos de prueba
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AplicationDbContext>();

    var admin = new proyectoIII.Models.Usuario
    {
        Nombre = "Administrador",
        Email = "admin@test.com",
        Password = "123",
        Rol = "Admin"
    };

    var docente = new proyectoIII.Models.Usuario
    {
        Nombre = "Profesor Carlos",
        Email = "carlos@test.com",
        Password = "123",
        Rol = "Docente"
    };

    var estudiante = new proyectoIII.Models.Usuario
    {
        Nombre = "Estudiante Ana",
        Email = "ana@test.com",
        Password = "123",
        Rol = "Estudiante"
    };

    context.Usuarios.AddRange(admin, docente, estudiante);
    context.SaveChanges();

    var curso = new proyectoIII.Models.Curso
    {
        Nombre = "Matemáticas",
        Descripcion = "Curso introductorio de matemáticas",
        DocenteId = docente.Id,
        FechaInicio = DateTime.Now,
        FechaFin = DateTime.Now.AddMonths(3)
    };

    context.Cursos.Add(curso);
    context.SaveChanges();

    var inscripcion = new proyectoIII.Models.Inscripcion
    {
        EstudianteId = estudiante.Id,
        CursoId = curso.Id
    };

    context.Inscripciones.Add(inscripcion);
    context.SaveChanges();
}

app.Run();