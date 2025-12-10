using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.Data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CalificacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("evaluacion/{evaluacionId}")]
        public async Task<ActionResult<IEnumerable<CalificacionDTO>>> GetCalificacionesByEvaluacion(int evaluacionId)
        {
            var calificaciones = await _context.Calificaciones
                .Where(c => c.EvaluacionId == evaluacionId)
                .Include(c => c.Estudiante)
                .Select(c => new CalificacionDTO
                {
                    Id = c.Id,
                    EvaluacionId = c.EvaluacionId,
                    EstudianteId = c.EstudianteId,
                    EstudianteNombre = c.Estudiante!.Nombre,
                    Puntaje = c.Puntaje,
                    Comentarios = c.Comentarios,
                    FechaCalificacion = c.FechaCalificacion
                })
                .ToListAsync();

            return Ok(calificaciones);
        }

        [HttpPost]
        public async Task<ActionResult<CalificacionDTO>> PostCalificacion(CalificacionCreacionDTO calificacionDTO)
        {
            var calificacion = new Calificacion
            {
                EvaluacionId = calificacionDTO.EvaluacionId,
                EstudianteId = calificacionDTO.EstudianteId,
                Puntaje = calificacionDTO.Puntaje,
                Comentarios = calificacionDTO.Comentarios
            };

            _context.Calificaciones.Add(calificacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCalificacionesByEvaluacion),
                new { evaluacionId = calificacion.EvaluacionId },
                calificacionDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalificacion(int id, CalificacionCreacionDTO calificacionDTO)
        {
            var calificacion = await _context.Calificaciones.FindAsync(id);
            if (calificacion == null) return NotFound();

            calificacion.Puntaje = calificacionDTO.Puntaje;
            calificacion.Comentarios = calificacionDTO.Comentarios;

            _context.Entry(calificacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
