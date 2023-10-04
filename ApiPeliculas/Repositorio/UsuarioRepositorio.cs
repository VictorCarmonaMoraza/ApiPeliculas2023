using ApiPeliculas.Data;
using ApiPeliculas.Helpers;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos.UsuarioDTO;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    /// <summary>
    /// Repositorio de Usuario
    /// </summary>
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        private readonly ApplicationDbContext _bd;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bd"></param>
        public UsuarioRepositorio(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        /// <summary>
        /// Obtenemos los usuario por su id
        /// </summary>
        /// <param name="usuarioId">id del usaurio</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Usuario GetUsuario(int usuarioId)
        {
            return _bd.Usuarios.FirstOrDefault(u=>u.Id == usuarioId);
        }

        /// <summary>
        /// Obtenemos lista de usaurios de la base de datos ordenados por el nombre
        /// </summary>
        /// <returns></returns>
        public ICollection<Usuario> GetUsuarios()
        {
            //Retornamos el nombre de usaurios ordenados por su nombre
            return _bd.Usuarios.OrderBy(u => u.Nombre).ToList();
        }

        /// <summary>
        /// Comporbar si el usaurio es unico
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool IsUniqueUser(string usuario)
        {
            var usuariobd = _bd.Usuarios.FirstOrDefault(u=>u.NombreUsuario == usuario);

            //Comporbamos si el usaurio de la base de datos es igual a null
            if (usuariobd == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Registramos un Usuario
        /// </summary>
        /// <param name="usuarioRegistroDto"></param>
        /// <returns></returns>
        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwwordEncriptada = EncriptarPassword.obtenermd5(usuarioRegistroDto.Password);

            Usuario usuario = new Usuario()
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = passwwordEncriptada,
                Nombre = usuarioRegistroDto.Nombre,
                Role = usuarioRegistroDto.Role,
            };

            //Guardado en BBDD
            _bd.Usuarios.Add(usuario);
            await _bd.SaveChangesAsync();
            usuario.Password = passwwordEncriptada;
            return usuario;
        }

        public Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            throw new NotImplementedException();
        }
    }
}
