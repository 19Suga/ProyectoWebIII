using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionesController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public InscripcionesController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<InscripcionDTO>>> GetInscripciones()
        {
            var inscripciones = await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .Where(i => i.Estado == "Activo")
                .Select(i => new InscripcionDTO
                {
                    Id = i.Id,
                    EstudianteId = i.EstudianteId,
                    EstudianteNombre = i.Estudiante.Nombre,
                    CursoId = i.CursoId,
                    CursoNombre = i.Curso.Nombre,
                    FechaInscripcion = i.FechaInscripcion,
                    Estado = i.Estado
                })
                .ToListAsync();

            return Ok(inscripciones);
        }

        [HttpPost]
        public async Task<ActionResult<InscripcionDTO>> PostInscripcion([FromBody] InscripcionDTO inscripcionDto)
        {
            var estudiante = await _context.Usuarios.FindAsync(inscripcionDto.EstudianteId);
            if (estudiante == null)
                return BadRequest("Estudiante no encontrado");

            var curso = await _context.Cursos.FindAsync(inscripcionDto.CursoId);
            if (curso == null)
                return BadRequest("Curso no encontrado");

            var existeInscripcion = await _context.Inscripciones
                .AnyAsync(i => i.EstudianteId == inscripcionDto.EstudianteId &&
                              i.CursoId == inscripcionDto.CursoId &&
                              i.Estado == "Activo");

            if (existeInscripcion)
                return BadRequest("estudiante ya está inscrito en este curso");

            var inscripcion = new Inscripcion
            {
                EstudianteId = inscripcionDto.EstudianteId,
                CursoId = inscripcionDto.CursoId,
                FechaInscripcion = DateTime.Now,
                Estado = "Activo"
            };

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            var responseDto = new InscripcionDTO
            {
                Id = inscripcion.Id,
                EstudianteId = inscripcion.EstudianteId,
                EstudianteNombre = estudiante.Nombre,
                CursoId = inscripcion.CursoId,
                CursoNombre = curso.Nombre,
                FechaInscripcion = inscripcion.FechaInscripcion,
                Estado = inscripcion.Estado
            };

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInscripcion(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion == null)
                return NotFound("Inscripción no encontrada");

            inscripcion.Estado = "Inactivo";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
