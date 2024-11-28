using Microsoft.EntityFrameworkCore;

namespace AuthWebApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación Usuario-Rol
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol) // Un usuario tiene un rol
                .WithMany(r => r.Usuarios) // Un rol tiene muchos usuarios
                .HasForeignKey(u => u.RolId) // Llave foránea
                .OnDelete(DeleteBehavior.Restrict); // Evita borrados en cascada de roles

            // Crear roles iniciales
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Admin" },
                new Rol { Id = 2, Nombre = "Usuario" }
            );
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }

    }
}
