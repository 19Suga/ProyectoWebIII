using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.Data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EvaluacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("curso/{cursoId}")]
        public async Task<ActionResult<IEnumerable<EvaluacionDTO>>> GetEvaluacionesByCurso(int cursoId)
        {
            var evaluaciones = await _context.Evaluaciones
                .Where(e => e.CursoId == cursoId)
                .Include(e => e.Creador)
                .Select(e => new EvaluacionDTO
                {
                    Id = e.Id,
                    CursoId = e.CursoId,
                    CreadorId = e.CreadorId,
                    CreadorNombre = e.Creador!.Nombre,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    Tipo = e.Tipo.ToString(),
                    Ponderacion = e.Ponderacion,
                    FechaDisponible = e.FechaDisponible,
                    FechaLimite = e.FechaLimite,
                    Activa = e.Activa
                })
                .ToListAsync();

            return Ok(evaluaciones);
        }

        [HttpPost]
        public async Task<ActionResult<EvaluacionDTO>> PostEvaluacion(EvaluacionCreacionDTO evaluacionDTO)
        {
            var evaluacion = new Evaluacion
            {
                CursoId = evaluacionDTO.CursoId,
                CreadorId = evaluacionDTO.CreadorId,
                Titulo = evaluacionDTO.Titulo,
                Descripcion = evaluacionDTO.Descripcion,
                Tipo = evaluacionDTO.Tipo,
                Ponderacion = evaluacionDTO.Ponderacion,
                FechaDisponible = evaluacionDTO.FechaDisponible,
                FechaLimite = evaluacionDTO.FechaLimite,
                Activa = true
            };

            _context.Evaluaciones.Add(evaluacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvaluacionesByCurso),
                new { cursoId = evaluacion.CursoId },
                evaluacionDTO);
        }
    }
}
