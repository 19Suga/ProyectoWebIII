namespace proyectoIII.Models
{
    public class Calificacion
    {
        public int Id { get; set; }
        public int EvaluacionId { get; set; }
        public Evaluacion Evaluacion { get; set; }
        public int EstudianteId { get; set; }
        public Usuario Estudiante { get; set; }
        public decimal PuntajeObtenido { get; set; }
        public DateTime FechaCalificacion { get; set; } = DateTime.Now;
        public string Comentarios { get; set; }
    }
}
