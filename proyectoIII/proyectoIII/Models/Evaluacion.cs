namespace proyectoIII.Models
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal PuntajeMaximo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaEntrega { get; set; }
        public int CursoId { get; set; }
        public Curso Curso { get; set; }
    }
}