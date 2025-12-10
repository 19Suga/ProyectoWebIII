namespace proyectoIII.Models.DTOs
{
    public class CursoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int DocenteId { get; set; }
        public string? DocenteNombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int CapacidadMaxima { get; set; }
        public int Inscritos { get; set; }
        public bool Activo { get; set; }
    }

    public class CursoCreacionDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int DocenteId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int CapacidadMaxima { get; set; }
    }
}
