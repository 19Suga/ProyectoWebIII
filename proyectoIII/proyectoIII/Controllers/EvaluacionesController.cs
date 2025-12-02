using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluacionesController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public EvaluacionesController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("curso/{cursoId}")]
        public async Task<ActionResult<List<Evaluacion>>> GetEvaluacionesPorCurso(int cursoId)
        {
            return await _context.Evaluaciones
                .Where(e => e.CursoId == cursoId)
                .Include(e => e.Curso)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Evaluacion>> PostEvaluacion(Evaluacion evaluacion, string rolUsuario, int usuarioId)
        {
            var curso = await _context.Cursos.FindAsync(evaluacion.CursoId);
            if (curso == null)
                return BadRequest("Curso no encontrado");

            if (rolUsuario != "Admin" && usuarioId != curso.DocenteId)
                return Unauthorized("Solo el docente del curso puede crear evaluaciones");

            evaluacion.FechaCreacion = DateTime.Now;
            _context.Evaluaciones.Add(evaluacion);
            await _context.SaveChangesAsync();

            return Ok(evaluacion);
        }

        [HttpPost("{evaluacionId}/calificar")]
        public async Task<ActionResult<Calificacion>> CalificarEstudiante(int evaluacionId, Calificacion calificacion, string rolUsuario, int usuarioId)
        {
            var evaluacion = await _context.Evaluaciones
                .Include(e => e.Curso)
                .FirstOrDefaultAsync(e => e.Id == evaluacionId);

            if (evaluacion == null)
                return BadRequest("Evaluación no encontrada");

            if (rolUsuario != "Admin" && usuarioId != evaluacion.Curso.DocenteId)
                return Unauthorized("Solo el docente del curso puede calificar");

            var estudiante = await _context.Usuarios.FindAsync(calificacion.EstudianteId);
            if (estudiante == null || estudiante.Rol != "Estudiante")
                return BadRequest("Solo estudiantes pueden ser calificados");

            calificacion.EvaluacionId = evaluacionId;
            calificacion.FechaCalificacion = DateTime.Now;

            _context.Calificaciones.Add(calificacion);
            await _context.SaveChangesAsync();

            return Ok(calificacion);
        }
    }
}

