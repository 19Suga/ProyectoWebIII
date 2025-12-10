using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoIII.Data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;
using System.Security.Cryptography;

namespace proyectoIII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Rol = u.Rol.ToString(),
                    Telefono = u.Telefono,
                    Activo = u.Activo,
                    FechaRegistro = u.FechaRegistro
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDTO = new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Telefono = usuario.Telefono,
                Activo = usuario.Activo,
                FechaRegistro = usuario.FechaRegistro
            };

            return usuarioDTO;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioRegistroDTO usuarioRegistroDTO)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuarioRegistroDTO.Email))
            {
                return Conflict("El email ya está registrado");
            }

            if (!Enum.TryParse<RolUsuario>(usuarioRegistroDTO.Rol, out var rol))
            {
                return BadRequest("Rol inválido");
            }

            var usuario = new Usuario
            {
                Nombre = usuarioRegistroDTO.Nombre,
                Email = usuarioRegistroDTO.Email,
                PasswordHash = HashPassword(usuarioRegistroDTO.Password),
                Rol = rol,
                Telefono = usuarioRegistroDTO.Telefono,
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var usuarioDTO = new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Telefono = usuario.Telefono,
                Activo = usuario.Activo,
                FechaRegistro = usuario.FechaRegistro
            };

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuarioDTO);
        }

        // POST: api/Usuarios/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDTO>> Login(UsuarioLoginDTO loginDTO)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (usuario == null || !VerifyPassword(loginDTO.Password, usuario.PasswordHash))
            {
                return Unauthorized("Credenciales inválidas");
            }

            if (!usuario.Activo)
            {
                return Unauthorized("Usuario inactivo");
            }

            var usuarioDTO = new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Telefono = usuario.Telefono,
                Activo = usuario.Activo,
                FechaRegistro = usuario.FechaRegistro
            };

            return Ok(usuarioDTO);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioRegistroDTO usuarioRegistroDTO)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            if (!Enum.TryParse<RolUsuario>(usuarioRegistroDTO.Rol, out var rol))
            {
                return BadRequest("Rol inválido");
            }

            usuario.Nombre = usuarioRegistroDTO.Nombre;
            usuario.Email = usuarioRegistroDTO.Email;
            usuario.Rol = rol;
            usuario.Telefono = usuarioRegistroDTO.Telefono;

            if (!string.IsNullOrEmpty(usuarioRegistroDTO.Password))
            {
                usuario.PasswordHash = HashPassword(usuarioRegistroDTO.Password);
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Activo = false;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
