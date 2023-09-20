using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Pelicula> GetPeliculas();

        Pelicula GetPelicula(int peliculaaId);

        bool ExistePelicula(string nombre);

        bool ExistePelicula(int id);

        bool CrearPelicula(Pelicula pelicula);

        bool ActualizarPelicula(Pelicula pelicula);

        bool BorrarPelicula(Pelicula pelicula);

        /// <summary>
        /// Metodos para buscar peliculas en Categori
        /// </summary>
        /// <param name="categoriaId">Id de la categoria</param>
        /// <returns></returns>
        ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId);

        /// <summary>
        /// Buscar pelicula por nombre
        /// </summary>
        /// <param name="nombrePelicula">nombre de la pelicula</param>
        /// <returns></returns>
        ICollection<Pelicula> BuscarPelicula(string nombrePelicula);

        bool Guardar();
    }
}
