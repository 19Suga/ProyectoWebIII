namespace proyectoIII.Models.DTOs
{
    public class MaterialDTO
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public int CreadorId { get; set; }
        public string? CreadorNombre { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? UrlArchivo { get; set; }
        public string? UrlRecurso { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class MaterialCreacionDTO
    {
        public int CursoId { get; set; }
        public int CreadorId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public TipoMaterial Tipo { get; set; }
        public string? UrlArchivo { get; set; }
        public string? UrlRecurso { get; set; }
    }
}
