using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoIII.Models
{
    public class Calificacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EvaluacionId { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Puntaje { get; set; }

        public string? Comentarios { get; set; }

        public DateTime FechaCalificacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        [ForeignKey("EvaluacionId")]
        public virtual Evaluacion? Evaluacion { get; set; }

        [ForeignKey("EstudianteId")]
        public virtual Usuario? Estudiante { get; set; }
    }
}
