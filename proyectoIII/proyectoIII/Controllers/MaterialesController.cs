using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.Data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MaterialesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("curso/{cursoId}")]
        public async Task<ActionResult<IEnumerable<MaterialDTO>>> GetMaterialesByCurso(int cursoId)
        {
            var materiales = await _context.MaterialesCursos
                .Where(m => m.CursoId == cursoId)
                .Include(m => m.Creador)
                .Select(m => new MaterialDTO
                {
                    Id = m.Id,
                    CursoId = m.CursoId,
                    CreadorId = m.CreadorId,
                    CreadorNombre = m.Creador!.Nombre,
                    Titulo = m.Titulo,
                    Descripcion = m.Descripcion,
                    Tipo = m.Tipo.ToString(),
                    UrlArchivo = m.UrlArchivo,
                    UrlRecurso = m.UrlRecurso,
                    FechaCreacion = m.FechaCreacion
                })
                .ToListAsync();

            return Ok(materiales);
        }

        [HttpPost]
        public async Task<ActionResult<MaterialDTO>> PostMaterial(MaterialCreacionDTO materialDTO)
        {
            var material = new MaterialCurso
            {
                CursoId = materialDTO.CursoId,
                CreadorId = materialDTO.CreadorId,
                Titulo = materialDTO.Titulo,
                Descripcion = materialDTO.Descripcion,
                Tipo = materialDTO.Tipo,
                UrlArchivo = materialDTO.UrlArchivo,
                UrlRecurso = materialDTO.UrlRecurso
            };

            _context.MaterialesCursos.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaterialesByCurso),
                new { cursoId = material.CursoId },
                materialDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.MaterialesCursos.FindAsync(id);
            if (material == null) return NotFound();

            _context.MaterialesCursos.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
