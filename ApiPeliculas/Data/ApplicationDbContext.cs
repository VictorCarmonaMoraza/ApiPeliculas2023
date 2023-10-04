using ApiPeliculas.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Agregar los modelos aqui
        
        //Modelo Categoria
        public DbSet<Categoria> Categoria { get; set; }

        //Modelo Pelicula
        public DbSet<Pelicula> Pelicula { get; set; }

        //Modelo Usuario
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
