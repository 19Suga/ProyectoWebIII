using Microsoft.EntityFrameworkCore;
using proyectoIII.Models;

namespace proyectoIII.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<MaterialCurso> MaterialesCursos { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }

       
    }
}
