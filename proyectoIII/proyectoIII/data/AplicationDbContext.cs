using Microsoft.EntityFrameworkCore;
using proyectoIII.Models;

namespace proyectoIII.data
{
    public class AplicationDbContext:DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
    }
}
