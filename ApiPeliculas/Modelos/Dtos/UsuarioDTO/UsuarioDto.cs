﻿namespace ApiPeliculas.Modelos.Dtos.UsuarioDTO
{
    /// <summary>
    /// DTO para el Usuario
    /// </summary>
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
