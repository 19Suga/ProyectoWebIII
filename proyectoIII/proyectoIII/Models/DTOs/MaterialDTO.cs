namespace proyectoIII.Models.DTOs
{
    public class MaterialDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string UrlArchivo { get; set; }
        public int CursoId { get; set; }
        public string CursoNombre { get; set; }
        public DateTime FechaSubida { get; set; }
    }
}
