namespace proyectoIII.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string UrlArchivo { get; set; }
        public int CursoId { get; set; }
        public Curso Curso { get; set; }
        public DateTime FechaSubida { get; set; } = DateTime.Now;
    }
}
