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
        public async Task<ActionResult<List<Inscripcion>>> GetInscripciones(string rolUsuario, int usuarioId)
        {
            if (rolUsuario == "Admin")
            {
                // Admin ve todo
                return await _context.Inscripciones
                    .Include(i => i.Estudiante)
                    .Include(i => i.Curso)
                    .Where(i => i.Estado == "Activo")
                    .ToListAsync();
            }
            else if (rolUsuario == "Docente")
            {
                return await _context.Inscripciones
                    .Include(i => i.Estudiante)
                    .Include(i => i.Curso)
                    .Where(i => i.Estado == "Activo" && i.Curso.DocenteId == usuarioId)
                    .ToListAsync();
            }
            else 
            {
                return await _context.Inscripciones
                    .Include(i => i.Curso)
                    .Where(i => i.EstudianteId == usuarioId && i.Estado == "Activo")
                    .ToListAsync();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Inscripcion>> PostInscripcion(Inscripcion inscripcion, string rolUsuario)
        {
            if (rolUsuario != "Admin")
                return Unauthorized("Solo administradores pueden inscribir estudiantes");

            var estudiante = await _context.Usuarios.FindAsync(inscripcion.EstudianteId);
            if (estudiante == null || estudiante.Rol != "Estudiante")
                return BadRequest("Solo estudiantes pueden ser inscritos");

            var curso = await _context.Cursos.FindAsync(inscripcion.CursoId);
            if (curso == null)
                return BadRequest("Curso no encontrado");

            var existe = await _context.Inscripciones
                .AnyAsync(i => i.EstudianteId == inscripcion.EstudianteId &&
                              i.CursoId == inscripcion.CursoId &&
                              i.Estado == "Activo");
            if (existe)
                return BadRequest("El estudiante ya está inscrito en este curso");

            inscripcion.FechaInscripcion = DateTime.Now;
            inscripcion.Estado = "Activo";

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            return Ok(inscripcion);
        }
    }
}
