namespace proyectoIII.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DocenteId { get; set; }
        public Usuario Docente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; } = true;

        // Relaciones actualizadas
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
        public ICollection<Materia> Materias { get; set; } = new List<Materia>();  // ← Cambiado
        public ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();
    }
}
