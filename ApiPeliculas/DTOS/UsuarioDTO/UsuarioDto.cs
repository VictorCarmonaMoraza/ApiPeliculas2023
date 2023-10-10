namespace ApiPeliculas.DTOS.UsuarioDTO
{
    /// <summary>
    /// DTO para el Usuario
    /// </summary>
    public class UsuarioDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
