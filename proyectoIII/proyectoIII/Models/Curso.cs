using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoIII.Models
{
    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required]
        public int DocenteId { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public int CapacidadMaxima { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        [ForeignKey("DocenteId")]
        public virtual Usuario? Docente { get; set; }

        public virtual ICollection<Inscripcion>? Inscripciones { get; set; }
        public virtual ICollection<MaterialCurso>? Materiales { get; set; }
        public virtual ICollection<Evaluacion>? Evaluaciones { get; set; }
    }
}
