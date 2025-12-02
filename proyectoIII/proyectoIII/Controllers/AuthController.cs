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
        public IActionResult Login(string email, string password)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u =>
                u.Email == email &&
                u.Password == password &&
                u.Activo);

            if (usuario == null)
                return Unauthorized("Credenciales inválidas");

            return Ok(new
            {
                id = usuario.Id,
                nombre = usuario.Nombre,
                email = usuario.Email,
                rol = usuario.Rol
            });
        }
    }
}
