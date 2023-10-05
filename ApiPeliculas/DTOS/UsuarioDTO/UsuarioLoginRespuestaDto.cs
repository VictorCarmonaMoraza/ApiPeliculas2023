using System.ComponentModel.DataAnnotations;
using ApiPeliculas.Modelos;

namespace ApiPeliculas.DTOS.UsuarioDTO
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
