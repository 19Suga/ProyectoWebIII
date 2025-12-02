namespace proyectoIII.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CursoId { get; set; }
        public Curso Curso { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
