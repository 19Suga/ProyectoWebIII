using System.Text;
using Microsoft.EntityFrameworkCore;
using proyectoIII.Data;
using proyectoIII.Models;
using proyectoIII.Models.DTOs;
using System.Security.Cryptography;

namespace proyectoIII.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioDTO?> Authenticate(UsuarioLoginDTO loginDTO)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (usuario == null || !VerifyPassword(loginDTO.Password, usuario.PasswordHash))
                return null;

            if (!usuario.Activo)
                return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Telefono = usuario.Telefono,
                Activo = usuario.Activo,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public async Task<UsuarioDTO> Register(UsuarioRegistroDTO registroDTO)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == registroDTO.Email))
                throw new Exception("El email ya está registrado");

            if (!Enum.TryParse<RolUsuario>(registroDTO.Rol, out var rol))
                throw new Exception("Rol inválido");

            var usuario = new Usuario
            {
                Nombre = registroDTO.Nombre,
                Email = registroDTO.Email,
                PasswordHash = HashPassword(registroDTO.Password),
                Rol = rol,
                Telefono = registroDTO.Telefono,
                Activo = true,
                FechaRegistro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Telefono = usuario.Telefono,
                Activo = usuario.Activo,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public async Task<bool> ChangePassword(int usuarioId, string currentPassword, string newPassword)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return false;

            if (!VerifyPassword(currentPassword, usuario.PasswordHash))
                return false;

            usuario.PasswordHash = HashPassword(newPassword);
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResetPassword(string email, string newPassword)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
                return false;

            usuario.PasswordHash = HashPassword(newPassword);
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProfile(int usuarioId, UsuarioRegistroDTO updateDTO)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return false;

            if (!Enum.TryParse<RolUsuario>(updateDTO.Rol, out var rol))
                return false;

            // Verificar si el email ya existe para otro usuario
            if (await _context.Usuarios.AnyAsync(u => u.Email == updateDTO.Email && u.Id != usuarioId))
                return false;

            usuario.Nombre = updateDTO.Nombre;
            usuario.Email = updateDTO.Email;
            usuario.Rol = rol;
            usuario.Telefono = updateDTO.Telefono;

            if (!string.IsNullOrEmpty(updateDTO.Password))
                usuario.PasswordHash = HashPassword(updateDTO.Password);

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UsuarioDTO?> GetUserById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Telefono = usuario.Telefono,
                Activo = usuario.Activo,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public async Task<List<UsuarioDTO>> GetUsersByRole(string role)
        {
            if (!Enum.TryParse<RolUsuario>(role, out var rol))
                return new List<UsuarioDTO>();

            return await _context.Usuarios
                .Where(u => u.Rol == rol)
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

        public string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            var random = new Random();
            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(chars);
        }

        public async Task<bool> ToggleUserStatus(int userId, bool isActive)
        {
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
                return false;

            usuario.Activo = isActive;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
