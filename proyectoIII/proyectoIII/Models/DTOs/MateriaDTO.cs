namespace proyectoIII.Models.DTOs
{
    public class MateriaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CursoId { get; set; }
        public string CursoNombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
