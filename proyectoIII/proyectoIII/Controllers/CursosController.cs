using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.Data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoDTO>>> GetCursos()
        {
            var cursos = await _context.Cursos
                .Include(c => c.Docente)
                .Select(c => new CursoDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    DocenteId = c.DocenteId,
                    DocenteNombre = c.Docente!.Nombre,
                    FechaInicio = c.FechaInicio,
                    FechaFin = c.FechaFin,
                    CapacidadMaxima = c.CapacidadMaxima,
                    Inscritos = c.Inscripciones!.Count(i => i.Estado == EstadoInscripcion.Aprobada),
                    Activo = c.Activo
                })
                .ToListAsync();

            return Ok(cursos);
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> GetCurso(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Docente)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
            {
                return NotFound();
            }

            var cursoDTO = new CursoDTO
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                Descripcion = curso.Descripcion,
                DocenteId = curso.DocenteId,
                DocenteNombre = curso.Docente!.Nombre,
                FechaInicio = curso.FechaInicio,
                FechaFin = curso.FechaFin,
                CapacidadMaxima = curso.CapacidadMaxima,
                Inscritos = curso.Inscripciones!.Count(i => i.Estado == EstadoInscripcion.Aprobada),
                Activo = curso.Activo
            };

            return Ok(cursoDTO);
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<CursoDTO>> PostCurso(CursoCreacionDTO cursoCreacionDTO)
        {
            var docente = await _context.Usuarios.FindAsync(cursoCreacionDTO.DocenteId);
            if (docente == null || docente.Rol != RolUsuario.Docente)
            {
                return BadRequest("El docente especificado no existe o no tiene rol de docente");
            }

            var curso = new Curso
            {
                Nombre = cursoCreacionDTO.Nombre,
                Descripcion = cursoCreacionDTO.Descripcion,
                DocenteId = cursoCreacionDTO.DocenteId,
                FechaInicio = cursoCreacionDTO.FechaInicio,
                FechaFin = cursoCreacionDTO.FechaFin,
                CapacidadMaxima = cursoCreacionDTO.CapacidadMaxima,
                Activo = true
            };

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            var cursoDTO = new CursoDTO
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                Descripcion = curso.Descripcion,
                DocenteId = curso.DocenteId,
                DocenteNombre = docente.Nombre,
                FechaInicio = curso.FechaInicio,
                FechaFin = curso.FechaFin,
                CapacidadMaxima = curso.CapacidadMaxima,
                Inscritos = 0,
                Activo = curso.Activo
            };

            return CreatedAtAction("GetCurso", new { id = curso.Id }, cursoDTO);
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, CursoCreacionDTO cursoCreacionDTO)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            curso.Nombre = cursoCreacionDTO.Nombre;
            curso.Descripcion = cursoCreacionDTO.Descripcion;
            curso.DocenteId = cursoCreacionDTO.DocenteId;
            curso.FechaInicio = cursoCreacionDTO.FechaInicio;
            curso.FechaFin = cursoCreacionDTO.FechaFin;
            curso.CapacidadMaxima = cursoCreacionDTO.CapacidadMaxima;

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            curso.Activo = false;
            _context.Entry(curso).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Cursos/docente/5
        [HttpGet("docente/{docenteId}")]
        public async Task<ActionResult<IEnumerable<CursoDTO>>> GetCursosByDocente(int docenteId)
        {
            var cursos = await _context.Cursos
                .Where(c => c.DocenteId == docenteId)
                .Include(c => c.Docente)
                .Select(c => new CursoDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    DocenteId = c.DocenteId,
                    DocenteNombre = c.Docente!.Nombre,
                    FechaInicio = c.FechaInicio,
                    FechaFin = c.FechaFin,
                    CapacidadMaxima = c.CapacidadMaxima,
                    Inscritos = c.Inscripciones!.Count(i => i.Estado == EstadoInscripcion.Aprobada),
                    Activo = c.Activo
                })
                .ToListAsync();

            return Ok(cursos);
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}
