namespace proyectoIII.Models.DTOs
{
    public class CalificacionDTO
    {
        public int Id { get; set; }
        public int EvaluacionId { get; set; }
        public string EvaluacionTitulo { get; set; }
        public int EstudianteId { get; set; }
        public string EstudianteNombre { get; set; }
        public decimal PuntajeObtenido { get; set; }
        public DateTime FechaCalificacion { get; set; }
        public string Comentarios { get; set; }
    }
}
