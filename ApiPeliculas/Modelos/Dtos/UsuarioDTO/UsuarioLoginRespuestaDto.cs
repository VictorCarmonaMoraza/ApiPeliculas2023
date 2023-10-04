using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos.UsuarioDTO
{
    /// <summary>
    /// Modelo de Usuario para cuando nos autentificamos correctamente
    /// </summary>
    public class UsuarioLoginRespuestaDto
    {
        public Usuario Usuario { get; set; }
        public string Token { get; set; }
    }
}
