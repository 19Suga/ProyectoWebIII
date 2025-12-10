namespace proyectoIII.Models.DTOs
{
    public class CalificacionDTO
    {
        public int Id { get; set; }
        public int EvaluacionId { get; set; }
        public int EstudianteId { get; set; }
        public string? EstudianteNombre { get; set; }
        public decimal Puntaje { get; set; }
        public string? Comentarios { get; set; }
        public DateTime FechaCalificacion { get; set; }
    }

    public class CalificacionCreacionDTO
    {
        public int EvaluacionId { get; set; }
        public int EstudianteId { get; set; }
        public decimal Puntaje { get; set; }
        public string? Comentarios { get; set; }
    }
}
