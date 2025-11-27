namespace proyectoIII.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public Usuario Estudiante { get; set; }
        public int CursoId { get; set; }
        public Curso Curso { get; set; }
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Activo";
    }
}
