using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public UsuariosController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Where(u => u.Activo)
                .Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Rol = u.Rol,
                    FechaCreacion = u.FechaCreacion
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario([FromBody] CreateUsuarioDTO createDto)
        {
            if (string.IsNullOrEmpty(createDto.Nombre) || string.IsNullOrEmpty(createDto.Email) || string.IsNullOrEmpty(createDto.Password))
                return BadRequest("Nombre, email y contraseña son obliagtoria");

            if (_context.Usuarios.Any(u => u.Email == createDto.Email))
                return BadRequest(" email ya está registrado");

            if (createDto.Rol != "Admin" && createDto.Rol != "Docente" && createDto.Rol != "Estudiante")
                return BadRequest("Rol son Admin, Docente o Estudiante");

            var usuario = new Usuario
            {
                Nombre = createDto.Nombre,
                Email = createDto.Email,
                Password = createDto.Password,
                Rol = createDto.Rol,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var usuarioDto = new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol,
                FechaCreacion = usuario.FechaCreacion
            };

            return Ok(usuarioDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado");

            usuario.Activo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
