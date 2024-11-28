using AuthWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {

        private readonly AppDbContext _context;

        public RolController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // Obtener un rol por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);

            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }

            return rol;
        }


        // Crear un nuevo rol
        [HttpPost]
        public async Task<ActionResult<Rol>> CreateRol([FromBody] Rol rol)
        {
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRol), new { id = rol.Id }, rol);
        }


        //// Actualizar un rol existente
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateRol(int id, [FromBody] Rol rol)
        //{
        //    if (id != rol.Id)
        //    {
        //        return BadRequest("El ID del rol no coincide.");
        //    }

        //    _context.Entry(rol).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RolExists(id))
        //        {
        //            return NotFound("Rol no encontrado.");
        //        }
        //        throw;
        //    }

        //    return NoContent();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] Rol rol)
        {
            if (rol == null)
            {
                return BadRequest("El cuerpo de la solicitud está vacío.");
            }

            // Asignar el ID desde la URL al objeto Rol
            rol.Id = id;

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
                {
                    return NotFound("Rol no encontrado.");
                }
                throw;
            }

            return Ok(new { message = "Rol actualizado correctamente." });
        }


        // Eliminar un rol
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Rol eliminado correctamente." });
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }



    }
}
