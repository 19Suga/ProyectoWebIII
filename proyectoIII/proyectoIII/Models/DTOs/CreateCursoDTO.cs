namespace proyectoIII.Models.DTOs
{
    public class CreateCursoDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DocenteId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
