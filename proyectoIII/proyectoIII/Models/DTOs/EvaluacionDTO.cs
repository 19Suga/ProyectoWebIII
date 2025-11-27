namespace proyectoIII.Models.DTOs
{
    public class EvaluacionDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal PuntajeMaximo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public int CursoId { get; set; }
        public string CursoNombre { get; set; }
    }
}
