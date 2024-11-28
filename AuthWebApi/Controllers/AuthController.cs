using AuthWebApi.DTOs;
using AuthWebApi.Models;
using AuthWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            this._context = context;
            this._tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("El correo ya está en uso.");
            }

            // Verificar que el rol proporcionado exista
            var rol = await _context.Roles.FindAsync(registerDto.RolId);
            if (rol == null)
            {
                return BadRequest("El rol proporcionado no existe.");
            }

            var usuario = new Usuario
            {
                Nombre = registerDto.Nombre,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                RolId = registerDto.RolId
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuario registrado con éxito.");
        }


        // Login de usuario
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash))
            {
                return Unauthorized("Credenciales inválidas.");
            }

            var token = _tokenService.GenerateToken(usuario);
            return Ok(new { Token = token });
        }
    }

}

