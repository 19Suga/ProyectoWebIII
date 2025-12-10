using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoIII.Models
{
    public enum TipoMaterial
    {
        Documento = 1,
        Video = 2,
        Enlace = 3,
        Presentacion = 4,
        Otro = 5
    }

    public class MaterialCurso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        public int CreadorId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required]
        public TipoMaterial Tipo { get; set; }

        [StringLength(500)]
        public string? UrlArchivo { get; set; }

        [StringLength(500)]
        public string? UrlRecurso { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        [ForeignKey("CursoId")]
        public virtual Curso? Curso { get; set; }

        [ForeignKey("CreadorId")]
        public virtual Usuario? Creador { get; set; }
    }
}