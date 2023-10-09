using ApiPeliculas.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext: IdentityDbContext<AppUsuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //Agregar los modelos aqui

        //Modelo Categoria
        public DbSet<Categoria> Categoria { get; set; }

        //Modelo Pelicula
        public DbSet<Pelicula> Pelicula { get; set; }

        //Modelo Usuario
        public DbSet<Usuario> Usuarios { get; set; }
        
        //Modelo AppUsuario
        public DbSet<AppUsuario> AppUsuario { get; set; }
    }
}
