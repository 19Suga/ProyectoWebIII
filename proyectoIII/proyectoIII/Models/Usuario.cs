using System.ComponentModel.DataAnnotations;

namespace proyectoIII.Models
{
    public enum RolUsuario
    {
        Admin = 1,
        Docente = 2,
        Estudiante = 3
    }

    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public RolUsuario Rol { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        // Relaciones
        public virtual ICollection<Curso>? CursosCreados { get; set; }
        public virtual ICollection<Inscripcion>? Inscripciones { get; set; }
        public virtual ICollection<Evaluacion>? EvaluacionesCreadas { get; set; }
    }
}
