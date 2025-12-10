using System.Text;
using proyectoIII.Models;

namespace proyectoIII.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            // Verificar si ya hay datos
            if (context.Usuarios.Any())
            {
                return; // La base de datos ya tiene datos
            }

            // Crear usuarios de prueba
            var usuarios = new Usuario[]
            {
                new Usuario
                {
                    Nombre = "Admin Principal",
                    Email = "admin@educativo.com",
                    PasswordHash = HashPassword("admin123"),
                    Rol = RolUsuario.Admin,
                    Telefono = "1234567890",
                    Activo = true
                },
                new Usuario
                {
                    Nombre = "Profesor Juan Pérez",
                    Email = "juan@educativo.com",
                    PasswordHash = HashPassword("profesor123"),
                    Rol = RolUsuario.Docente,
                    Telefono = "0987654321",
                    Activo = true
                },
                new Usuario
                {
                    Nombre = "Profesora María García",
                    Email = "maria@educativo.com",
                    PasswordHash = HashPassword("profesor123"),
                    Rol = RolUsuario.Docente,
                    Telefono = "1122334455",
                    Activo = true
                },
                new Usuario
                {
                    Nombre = "Estudiante Ana López",
                    Email = "ana@educativo.com",
                    PasswordHash = HashPassword("estudiante123"),
                    Rol = RolUsuario.Estudiante,
                    Telefono = "5566778899",
                    Activo = true
                },
                new Usuario
                {
                    Nombre = "Estudiante Carlos Ruiz",
                    Email = "carlos@educativo.com",
                    PasswordHash = HashPassword("estudiante123"),
                    Rol = RolUsuario.Estudiante,
                    Telefono = "6677889900",
                    Activo = true
                },
                new Usuario
                {
                    Nombre = "Estudiante Sofía Méndez",
                    Email = "sofia@educativo.com",
                    PasswordHash = HashPassword("estudiante123"),
                    Rol = RolUsuario.Estudiante,
                    Telefono = "7788990011",
                    Activo = true
                }
            };

            await context.Usuarios.AddRangeAsync(usuarios);
            await context.SaveChangesAsync();

            // Obtener IDs de usuarios creados
            var admin = usuarios[0];
            var profesorJuan = usuarios[1];
            var profesoraMaria = usuarios[2];
            var estudianteAna = usuarios[3];
            var estudianteCarlos = usuarios[4];
            var estudianteSofia = usuarios[5];

            // Crear cursos de prueba
            var cursos = new Curso[]
            {
                new Curso
                {
                    Nombre = "Programación en C#",
                    Descripcion = "Curso introductorio de programación en C# y .NET",
                    DocenteId = profesorJuan.Id,
                    FechaInicio = DateTime.Now.AddDays(-30),
                    FechaFin = DateTime.Now.AddDays(60),
                    CapacidadMaxima = 30,
                    Activo = true
                },
                new Curso
                {
                    Nombre = "Base de Datos SQL",
                    Descripcion = "Fundamentos de bases de datos relacionales y SQL",
                    DocenteId = profesorJuan.Id,
                    FechaInicio = DateTime.Now.AddDays(-15),
                    FechaFin = DateTime.Now.AddDays(75),
                    CapacidadMaxima = 25,
                    Activo = true
                },
                new Curso
                {
                    Nombre = "Desarrollo Web con ASP.NET",
                    Descripcion = "Desarrollo de aplicaciones web con ASP.NET Core",
                    DocenteId = profesoraMaria.Id,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddDays(90),
                    CapacidadMaxima = 35,
                    Activo = true
                },
                new Curso
                {
                    Nombre = "Matemáticas Discretas",
                    Descripcion = "Fundamentos matemáticos para ciencias de la computación",
                    DocenteId = profesoraMaria.Id,
                    FechaInicio = DateTime.Now.AddDays(-45),
                    FechaFin = DateTime.Now.AddDays(45),
                    CapacidadMaxima = 40,
                    Activo = true
                }
            };

            await context.Cursos.AddRangeAsync(cursos);
            await context.SaveChangesAsync();

            // crear inscripciones de prueba
            var inscripciones = new Inscripcion[]
            {
                new Inscripcion { EstudianteId = estudianteAna.Id, CursoId = cursos[0].Id, Estado = EstadoInscripcion.Aprobada, FechaAprobacion = DateTime.Now.AddDays(-28) },
                new Inscripcion { EstudianteId = estudianteCarlos.Id, CursoId = cursos[0].Id, Estado = EstadoInscripcion.Aprobada, FechaAprobacion = DateTime.Now.AddDays(-27) },
                new Inscripcion { EstudianteId = estudianteSofia.Id, CursoId = cursos[0].Id, Estado = EstadoInscripcion.Aprobada, FechaAprobacion = DateTime.Now.AddDays(-26) },
                
                new Inscripcion { EstudianteId = estudianteAna.Id, CursoId = cursos[1].Id, Estado = EstadoInscripcion.Aprobada, FechaAprobacion = DateTime.Now.AddDays(-13) },
                new Inscripcion { EstudianteId = estudianteCarlos.Id, CursoId = cursos[1].Id, Estado = EstadoInscripcion.Pendiente },
                
                new Inscripcion { EstudianteId = estudianteSofia.Id, CursoId = cursos[2].Id, Estado = EstadoInscripcion.Aprobada, FechaAprobacion = DateTime.Now.AddDays(-1) },
                new Inscripcion { EstudianteId = estudianteAna.Id, CursoId = cursos[3].Id, Estado = EstadoInscripcion.Aprobada, FechaAprobacion = DateTime.Now.AddDays(-43) },
                new Inscripcion { EstudianteId = estudianteCarlos.Id, CursoId = cursos[3].Id, Estado = EstadoInscripcion.Aprobada, FechaAprobacion = DateTime.Now.AddDays(-42) },
                new Inscripcion { EstudianteId = estudianteSofia.Id, CursoId = cursos[3].Id, Estado = EstadoInscripcion.Rechazada }
            };

            await context.Inscripciones.AddRangeAsync(inscripciones);
            await context.SaveChangesAsync();

            // Crear materiales de curso de prueba
            var materiales = new MaterialCurso[]
            {
                new MaterialCurso
                {
                    CursoId = cursos[0].Id,
                    CreadorId = profesorJuan.Id,
                    Titulo = "Introducción a C#",
                    Descripcion = "Presentación inicial del curso",
                    Tipo = TipoMaterial.Presentacion,
                    UrlRecurso = "https://docs.microsoft.com/es-es/dotnet/csharp/"
                },
                new MaterialCurso
                {
                    CursoId = cursos[0].Id,
                    CreadorId = profesorJuan.Id,
                    Titulo = "Ejercicios Prácticos Semana 1",
                    Descripcion = "Ejercicios para practicar los conceptos básicos",
                    Tipo = TipoMaterial.Documento
                },
                new MaterialCurso
                {
                    CursoId = cursos[2].Id,
                    CreadorId = profesoraMaria.Id,
                    Titulo = "Video: Introducción a ASP.NET Core",
                    Descripcion = "Video tutorial sobre los fundamentos",
                    Tipo = TipoMaterial.Video,
                    UrlRecurso = "https://youtube.com/watch?v=ejemplo"
                }
            };

            await context.MaterialesCursos.AddRangeAsync(materiales);
            await context.SaveChangesAsync();

            // Crear evaluaciones de prueba
            var evaluaciones = new Evaluacion[]
            {
                new Evaluacion
                {
                    CursoId = cursos[0].Id,
                    CreadorId = profesorJuan.Id,
                    Titulo = "Examen Parcial 1",
                    Descripcion = "Evaluación de los primeros 3 temas",
                    Tipo = TipoEvaluacion.Examen,
                    Ponderacion = 30,
                    FechaDisponible = DateTime.Now.AddDays(-20),
                    FechaLimite = DateTime.Now.AddDays(-10),
                    Activa = false
                },
                new Evaluacion
                {
                    CursoId = cursos[0].Id,
                    CreadorId = profesorJuan.Id,
                    Titulo = "Tarea: Programas Básicos",
                    Descripcion = "Desarrollo de 5 programas básicos en C#",
                    Tipo = TipoEvaluacion.Tarea,
                    Ponderacion = 20,
                    FechaDisponible = DateTime.Now.AddDays(-15),
                    FechaLimite = DateTime.Now.AddDays(5),
                    Activa = true
                },
                new Evaluacion
                {
                    CursoId = cursos[3].Id,
                    CreadorId = profesoraMaria.Id,
                    Titulo = "Quiz: Lógica Proposicional",
                    Descripcion = "Evaluación rápida de conceptos básicos",
                    Tipo = TipoEvaluacion.Quiz,
                    Ponderacion = 10,
                    FechaDisponible = DateTime.Now.AddDays(-30),
                    FechaLimite = DateTime.Now.AddDays(-20),
                    Activa = false
                }
            };

            await context.Evaluaciones.AddRangeAsync(evaluaciones);
            await context.SaveChangesAsync();

            // Crear calificaciones de prueba
            var calificaciones = new Calificacion[]
            {
                new Calificacion
                {
                    EvaluacionId = evaluaciones[0].Id,
                    EstudianteId = estudianteAna.Id,
                    Puntaje = 85,
                    Comentarios = "Excelente trabajo"
                },
                new Calificacion
                {
                    EvaluacionId = evaluaciones[0].Id,
                    EstudianteId = estudianteCarlos.Id,
                    Puntaje = 92,
                    Comentarios = "Muy buen desempeño"
                },
                new Calificacion
                {
                    EvaluacionId = evaluaciones[0].Id,
                    EstudianteId = estudianteSofia.Id,
                    Puntaje = 78,
                    Comentarios = "Puede mejorar"
                },
                new Calificacion
                {
                    EvaluacionId = evaluaciones[2].Id,
                    EstudianteId = estudianteAna.Id,
                    Puntaje = 95,
                    Comentarios = "Perfecto"
                },
                new Calificacion
                {
                    EvaluacionId = evaluaciones[2].Id,
                    EstudianteId = estudianteCarlos.Id,
                    Puntaje = 88,
                    Comentarios = "Buen trabajo"
                }
            };

            await context.Calificaciones.AddRangeAsync(calificaciones);
            await context.SaveChangesAsync();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
