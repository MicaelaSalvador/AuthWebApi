using System.Text.Json.Serialization;

namespace AuthWebApi.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // Por ejemplo: "Admin", "Usuario"

        // Relación inversa: Un rol puede tener muchos usuarios

        [JsonIgnore]
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
