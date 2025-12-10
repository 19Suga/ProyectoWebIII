using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.Data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InscripcionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Inscripciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionDTO>>> GetInscripciones()
        {
            var inscripciones = await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .Select(i => new InscripcionDTO
                {
                    Id = i.Id,
                    EstudianteId = i.EstudianteId,
                    EstudianteNombre = i.Estudiante!.Nombre,
                    CursoId = i.CursoId,
                    CursoNombre = i.Curso!.Nombre,
                    FechaInscripcion = i.FechaInscripcion,
                    Estado = i.Estado.ToString(),
                    FechaAprobacion = i.FechaAprobacion
                })
                .ToListAsync();

            return Ok(inscripciones);
        }

        // POST: api/Inscripciones
        [HttpPost]
        public async Task<ActionResult<InscripcionDTO>> PostInscripcion(InscripcionCreacionDTO inscripcionCreacionDTO)
        {
            var estudiante = await _context.Usuarios.FindAsync(inscripcionCreacionDTO.EstudianteId);
            if (estudiante == null || estudiante.Rol != RolUsuario.Estudiante)
            {
                return BadRequest("El estudiante especificado no existe o no tiene rol de estudiante");
            }

            var curso = await _context.Cursos.FindAsync(inscripcionCreacionDTO.CursoId);
            if (curso == null || !curso.Activo)
            {
                return BadRequest("El curso especificado no existe o está inactivo");
            }

            var inscripcionesAprobadas = await _context.Inscripciones
                .CountAsync(i => i.CursoId == inscripcionCreacionDTO.CursoId &&
                               i.Estado == EstadoInscripcion.Aprobada);

            if (inscripcionesAprobadas >= curso.CapacidadMaxima)
            {
                return BadRequest("El curso ha alcanzado su capacidad máxima");
            }

            var inscripcionExistente = await _context.Inscripciones
                .FirstOrDefaultAsync(i => i.EstudianteId == inscripcionCreacionDTO.EstudianteId &&
                                        i.CursoId == inscripcionCreacionDTO.CursoId);

            if (inscripcionExistente != null)
            {
                return Conflict("El estudiante ya está inscrito en este curso");
            }

            var inscripcion = new Inscripcion
            {
                EstudianteId = inscripcionCreacionDTO.EstudianteId,
                CursoId = inscripcionCreacionDTO.CursoId,
                Estado = EstadoInscripcion.Pendiente
            };

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            var inscripcionDTO = new InscripcionDTO
            {
                Id = inscripcion.Id,
                EstudianteId = inscripcion.EstudianteId,
                EstudianteNombre = estudiante.Nombre,
                CursoId = inscripcion.CursoId,
                CursoNombre = curso.Nombre,
                FechaInscripcion = inscripcion.FechaInscripcion,
                Estado = inscripcion.Estado.ToString(),
                FechaAprobacion = inscripcion.FechaAprobacion
            };

            return CreatedAtAction("GetInscripcion", new { id = inscripcion.Id }, inscripcionDTO);
        }

        // GET: api/Inscripciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InscripcionDTO>> GetInscripcion(int id)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inscripcion == null)
            {
                return NotFound();
            }

            var inscripcionDTO = new InscripcionDTO
            {
                Id = inscripcion.Id,
                EstudianteId = inscripcion.EstudianteId,
                EstudianteNombre = inscripcion.Estudiante!.Nombre,
                CursoId = inscripcion.CursoId,
                CursoNombre = inscripcion.Curso!.Nombre,
                FechaInscripcion = inscripcion.FechaInscripcion,
                Estado = inscripcion.Estado.ToString(),
                FechaAprobacion = inscripcion.FechaAprobacion
            };

            return Ok(inscripcionDTO);
        }

        // PUT: api/Inscripciones/5/aprobar
        [HttpPut("{id}/aprobar")]
        public async Task<IActionResult> AprobarInscripcion(int id)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Curso)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inscripcion == null)
            {
                return NotFound();
            }

            if (inscripcion.Estado == EstadoInscripcion.Aprobada)
            {
                return BadRequest("La inscripción ya está aprobada");
            }

            var inscripcionesAprobadas = await _context.Inscripciones
                .CountAsync(i => i.CursoId == inscripcion.CursoId &&
                               i.Estado == EstadoInscripcion.Aprobada);

            if (inscripcionesAprobadas >= inscripcion.Curso!.CapacidadMaxima)
            {
                return BadRequest("El curso ha alcanzado su capacidad máxima");
            }

            inscripcion.Estado = EstadoInscripcion.Aprobada;
            inscripcion.FechaAprobacion = DateTime.UtcNow;

            _context.Entry(inscripcion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Inscripciones/5/rechazar
        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> RechazarInscripcion(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            if (inscripcion.Estado == EstadoInscripcion.Rechazada)
            {
                return BadRequest("La inscripción ya está rechazada");
            }

            inscripcion.Estado = EstadoInscripcion.Rechazada;
            _context.Entry(inscripcion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Inscripciones/estudiante/5
        [HttpGet("estudiante/{estudianteId}")]
        public async Task<ActionResult<IEnumerable<InscripcionDTO>>> GetInscripcionesByEstudiante(int estudianteId)
        {
            var inscripciones = await _context.Inscripciones
                .Where(i => i.EstudianteId == estudianteId)
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .Select(i => new InscripcionDTO
                {
                    Id = i.Id,
                    EstudianteId = i.EstudianteId,
                    EstudianteNombre = i.Estudiante!.Nombre,
                    CursoId = i.CursoId,
                    CursoNombre = i.Curso!.Nombre,
                    FechaInscripcion = i.FechaInscripcion,
                    Estado = i.Estado.ToString(),
                    FechaAprobacion = i.FechaAprobacion
                })
                .ToListAsync();

            return Ok(inscripciones);
        }

        // GET: api/Inscripciones/curso/5
        [HttpGet("curso/{cursoId}")]
        public async Task<ActionResult<IEnumerable<InscripcionDTO>>> GetInscripcionesByCurso(int cursoId)
        {
            var inscripciones = await _context.Inscripciones
                .Where(i => i.CursoId == cursoId)
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .Select(i => new InscripcionDTO
                {
                    Id = i.Id,
                    EstudianteId = i.EstudianteId,
                    EstudianteNombre = i.Estudiante!.Nombre,
                    CursoId = i.CursoId,
                    CursoNombre = i.Curso!.Nombre,
                    FechaInscripcion = i.FechaInscripcion,
                    Estado = i.Estado.ToString(),
                    FechaAprobacion = i.FechaAprobacion
                })
                .ToListAsync();

            return Ok(inscripciones);
        }

        // DELETE: api/Inscripciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInscripcion(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
