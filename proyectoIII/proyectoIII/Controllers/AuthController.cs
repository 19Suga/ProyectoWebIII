    using Microsoft.AspNetCore.Mvc;
using proyectoIII.data;
using proyectoIII.Models.DTOs;

namespace proyectoIII.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public AuthController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
                return BadRequest("Obligado llenar todos los campos");

            var usuario = _context.Usuarios.FirstOrDefault(u =>
                u.Email == loginDTO.Email &&
                u.Password == loginDTO.Password &&
                u.Activo);

            if (usuario == null)
                return Unauthorized("datos invalidos");

            return Ok(new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol,
                FechaCreacion = usuario.FechaCreacion
            });
        }
    }
}
