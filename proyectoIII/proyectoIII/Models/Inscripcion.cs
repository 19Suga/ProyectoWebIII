using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoIII.Models
{
    public enum EstadoInscripcion
    {
        Pendiente = 1,
        Aprobada = 2,
        Rechazada = 3,
        Finalizada = 4
    }

    public class Inscripcion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;

        public EstadoInscripcion Estado { get; set; } = EstadoInscripcion.Pendiente;

        public DateTime? FechaAprobacion { get; set; }

        // Relaciones
        [ForeignKey("EstudianteId")]
        public virtual Usuario? Estudiante { get; set; }

        [ForeignKey("CursoId")]
        public virtual Curso? Curso { get; set; }
    }
}
