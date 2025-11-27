using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public CursosController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CursoDTO>>> GetCursos()
        {
            var cursos = await _context.Cursos
                .Where(c => c.Activo)
                .Include(c => c.Docente)
                .Select(c => new CursoDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    DocenteId = c.DocenteId,
                    DocenteNombre = c.Docente.Nombre,
                    FechaInicio = c.FechaInicio,
                    FechaFin = c.FechaFin,
                    CantidadEstudiantes = c.Inscripciones.Count(i => i.Estado == "Activo")
                })
                .ToListAsync();

            return Ok(cursos);
        }

        [HttpPost]
        public async Task<ActionResult<CursoDTO>> PostCurso([FromBody] CreateCursoDTO createDto)
        {
            if (string.IsNullOrEmpty(createDto.Nombre))
                return BadRequest("El nombre del curso obligatorio");

            if (createDto.FechaFin <= createDto.FechaInicio)
                return BadRequest("La fecha de fin debe ser posterior a la fecha de inicio");

            var docente = await _context.Usuarios.FindAsync(createDto.DocenteId);
            if (docente == null)
                return BadRequest("Docente no encontrado");

            var curso = new Curso
            {
                Nombre = createDto.Nombre,
                Descripcion = createDto.Descripcion,
                DocenteId = createDto.DocenteId,
                FechaInicio = createDto.FechaInicio,
                FechaFin = createDto.FechaFin,
                Activo = true
            };

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            var cursoDto = new CursoDTO
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                Descripcion = curso.Descripcion,
                DocenteId = curso.DocenteId,
                DocenteNombre = docente.Nombre,
                FechaInicio = curso.FechaInicio,
                FechaFin = curso.FechaFin,
                CantidadEstudiantes = 0
            };

            return Ok(cursoDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound("Curso no encontrado");

            curso.Activo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
