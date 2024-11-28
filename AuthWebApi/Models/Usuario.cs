using System.Text.Json.Serialization;

namespace AuthWebApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Llave foránea para Rol
        public int RolId { get; set; }

        [JsonIgnore] // Ignorar al recibir el cuerpo de la solicitud
        public Rol Rol { get; set; } // Propiedad de navegación
    }
}
