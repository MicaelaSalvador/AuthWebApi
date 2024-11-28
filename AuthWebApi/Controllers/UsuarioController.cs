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
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public UsuarioController(AppDbContext context, IConfiguration configuration, TokenService tokenService)
        {
            this._context = context;
            this._configuration = configuration;
            this._tokenService = tokenService;
        }

        // Obtener todos los usuarios
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        //{
        //    return await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();

            var usuarioDtos = usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                RolNombre = u.Rol.Nombre
            });

            return Ok(usuarioDtos);
        }



        // Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return usuario;
        }


        //// Actualizar un usuario existente
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
        //{
        //    if (id != usuario.Id)
        //    {
        //        return BadRequest("El ID del usuario no coincide.");
        //    }

        //    // Validar que el rol exista
        //    var rol = await _context.Roles.FindAsync(usuario.RolId);
        //    if (rol == null)
        //    {
        //        return BadRequest("El rol especificado no existe.");
        //    }

        //    _context.Entry(usuario).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UsuarioExists(id))
        //        {
        //            return NotFound("Usuario no encontrado.");
        //        }
        //        throw;
        //    }

        //    return NoContent();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto usuarioDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Actualizar los campos permitidos
            usuario.Nombre = usuarioDto.Nombre;
            usuario.Email = usuarioDto.Email;

            // Si se proporciona una nueva contraseña, encriptarla
            if (!string.IsNullOrWhiteSpace(usuarioDto.Password))
            {
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password);
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
                throw;
            }

            return Ok(new { message = "Usuario  actualizado correctamente." });
        }


        // Eliminar un usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario eliminado correctamente." });
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

    }
}
