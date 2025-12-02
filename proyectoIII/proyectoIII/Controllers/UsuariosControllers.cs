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
        public async Task<ActionResult<List<Usuario>>> GetUsuarios(string rolUsuario)
        {
            if (rolUsuario != "Admin")
                return Unauthorized("Solo administradores pueden ver usuarios");

            return await _context.Usuarios.Where(u => u.Activo).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario, string rolUsuarioQueCrea)
        {
            if (usuario.Rol == "Admin" && rolUsuarioQueCrea != "Admin")
                return BadRequest("Solo administradores pueden crear otros administradores");

            usuario.FechaCreacion = DateTime.Now;
            usuario.Activo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }
    }
}
