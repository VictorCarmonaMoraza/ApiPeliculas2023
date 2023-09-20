using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _bd;

        //Constructor
        public PeliculaRepositorio(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        /// <summary>
        /// Actualiza una pelicula
        /// </summary>
        /// <param name="pelicula">modelo Pelicula</param>
        /// <returns></returns>
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            //Actualizamos la fecha de actualizacion
            pelicula.FechaCreacion = DateTime.Now;
            //Actualizamos los cambios
            _bd.Pelicula.Update(pelicula);
            //Guardamos los cambios
            return Guardar();

        }

        /// <summary>
        /// Borramos la pelicula
        /// </summary>
        /// <param name="pelicula">Modelo Pelicula</param>
        /// <returns></returns>
        public bool BorrarPelicula(Pelicula pelicula)
        {
            //Borramos la pelicula
            _bd.Pelicula.Remove(pelicula);
            //Guardamos los cambios
            return Guardar();
        }

        /// <summary>
        /// Creacion de una peplicula
        /// </summary>
        /// <param name="pelicula">Modelo Pelicula</param>
        /// <returns></returns>
        public bool CrearPelicula(Pelicula pelicula)
        {
            //Creamos la fecha de la pelicula
            pelicula.FechaCreacion = DateTime.Now;
            //Añadimos la Pelicula a la tabla Pelicula
            _bd.Pelicula.Add(pelicula);
            //Guardamos los cambios
            return Guardar();
        }

        /// <summary>
        /// Validamos si existe la pelicula por su nombre
        /// </summary>
        /// <param name="nombre">nombre de la pelicula</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ExistePelicula(string nombre)
        {
            bool valor = _bd.Pelicula.Any(c=>c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        /// <summary>
        /// Validamos si existe una pelicula por su id
        /// </summary>
        /// <param name="id">id de la pelicula</param>
        /// <returns></returns>
        public bool ExistePelicula(int id)
        {
            return _bd.Pelicula.Any(c => c.Id == id);
        }

        /// <summary>
        /// Obtenemos una pelicula por su id
        /// </summary>
        /// <param name="peliculaaId">id de la pelicula</param>
        /// <returns></returns>
        public Pelicula GetPelicula(int peliculaaId)
        {
            //Obtenemos una pelicula por su id
            return _bd.Pelicula.FirstOrDefault(c => c.Id == peliculaaId);
        }

        /// <summary>
        /// Obtiene listado de peliculas
        /// </summary>
        /// <returns></returns>
        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.Pelicula.OrderBy(c => c.Nombre).ToList();
        }

        /// <summary>
        /// Busca peliculas por su id
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <returns></returns>
        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
        {
            return _bd.Pelicula.Include(c => c.Categoria).Where(ca=>ca.categoriaId ==categoriaId).ToList();

        }

        /// <summary>
        /// Buscar una categoria por su nombre
        /// </summary>
        /// <param name="nombrePelicula"></param>
        /// <returns></returns>
        public ICollection<Pelicula> BuscarPelicula(string nombrePelicula)
        {
            //return _bd.Pelicula.Where(c=>c.Nombre.ToLower().Trim() == nombrePelicula.ToLower().Trim()).ToList();
            IQueryable<Pelicula> query = _bd.Pelicula;
            //Sino no es vavio el nombre de la pelicula
            if (!string.IsNullOrEmpty(nombrePelicula))
            {
                query = query.Where(e=>e.Nombre.Contains(nombrePelicula) || e.Descripcion.Contains(nombrePelicula));
            }
            return query.ToList();
        }

        public bool Guardar() => _bd.SaveChanges() >= 0 ? true : false;

    }
}
