using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriasController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public MateriasController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Materia>>> GetMaterias()
        {
            return await _context.Materias
                .Include(m => m.Curso)
                .ToListAsync();
        }

        [HttpGet("curso/{cursoId}")]
        public async Task<ActionResult<List<Materia>>> GetMateriasPorCurso(int cursoId)
        {
            return await _context.Materias
                .Where(m => m.CursoId == cursoId)
                .Include(m => m.Curso)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Materia>> PostMateria(Materia materia, string rolUsuario, int usuarioId)
        {
            var curso = await _context.Cursos.FindAsync(materia.CursoId);
            if (curso == null)
                return BadRequest("Curso no encontrado");

            if (rolUsuario != "Admin" && usuarioId != curso.DocenteId)
                return Unauthorized("Solo el docente del curso puede crear materias");

            materia.FechaCreacion = DateTime.Now;
            _context.Materias.Add(materia);
            await _context.SaveChangesAsync();

            return Ok(materia);
        }
    }
}
