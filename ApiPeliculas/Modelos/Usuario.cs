using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    /// <summary>
    /// Modelo de Usuario
    /// </summary>
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Password {  get; set; }
        public string Role { get; set; }
    }
}
