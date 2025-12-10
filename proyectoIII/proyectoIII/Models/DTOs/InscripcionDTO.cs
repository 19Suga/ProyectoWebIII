namespace proyectoIII.Models.DTOs
{
    public class InscripcionDTO
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public string? EstudianteNombre { get; set; }
        public int CursoId { get; set; }
        public string? CursoNombre { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime? FechaAprobacion { get; set; }
    }

    public class InscripcionCreacionDTO
    {
        public int EstudianteId { get; set; }
        public int CursoId { get; set; }
    }
}
