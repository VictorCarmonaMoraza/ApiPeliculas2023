using ApiPeliculas.DTOS.CategoriaDTO;
using ApiPeliculas.DTOS.PeliculaDTO;
using ApiPeliculas.DTOS.UsuarioDTO;
using ApiPeliculas.Modelos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
    /// <summary>
    /// Mapeo de Usuario
    /// </summary>
    public class UsuariosMapper : Profile
    {
        public UsuariosMapper()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
        }
    }
}
