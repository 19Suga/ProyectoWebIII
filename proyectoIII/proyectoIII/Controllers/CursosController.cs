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
        public async Task<ActionResult<List<Curso>>> GetCursos()
        {
            return await _context.Cursos
                .Where(c => c.Activo)
                .Include(c => c.Docente)
                .ToListAsync();
        }

        [HttpGet("docente/{docenteId}")]
        public async Task<ActionResult<List<Curso>>> GetCursosPorDocente(int docenteId, string rolUsuario, int usuarioId)
        {
            if (rolUsuario != "Admin" && usuarioId != docenteId)
                return Unauthorized("Solo puedes ver tus propios cursos");

            return await _context.Cursos
                .Where(c => c.DocenteId == docenteId && c.Activo)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso, string rolUsuario)
        {
            if (rolUsuario != "Admin")
                return Unauthorized("Solo administradores pueden crear cursos");

            var docente = await _context.Usuarios.FindAsync(curso.DocenteId);
            if (docente == null || docente.Rol != "Docente")
                return BadRequest("Docente no válido");

            curso.Activo = true;
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return Ok(curso);
        }
    }
}
