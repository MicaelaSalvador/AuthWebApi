namespace AuthWebApi.DTOs
{
    public class RegisterDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RolId { get; set; } // Id del rol que se asignará al usuario
    }
}
