using Microsoft.EntityFrameworkCore;
using proyectoIII.Models;

namespace proyectoIII.data
{
    public class AplicationDbContext:DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
