using ApiPeliculas.DTOS.UsuarioDTO;
using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    /// <summary>
    /// Interfza de Usuario
    /// </summary>
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Obtiene coleccion de Usaurios
        /// </summary>
        /// <returns></returns>
        ICollection<AppUsuario> GetUsuarios();

        /// <summary>
        /// Obytiene el usuario por su id
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        AppUsuario GetUsuario(string usuarioId);

        /// <summary>
        /// Comporbamos si el usuario es unico
        /// </summary>
        /// <param name="usuario">usuario a buscar</param>
        /// <returns></returns>
        bool IsUniqueUser(string usuario);

        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);

        /// <summary>
        /// Registro de un usuario
        /// </summary>
        /// <param name="usuarioRegistroDto">usuario a registrar</param>
        /// <returns></returns>
        //Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}
