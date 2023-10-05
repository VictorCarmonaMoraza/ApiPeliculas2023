using ApiPeliculas.DTOS.CategoriaDTO;
using ApiPeliculas.DTOS.PeliculaDTO;
using ApiPeliculas.Modelos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
                CreateMap<Categoria, CategoriaDto>().ReverseMap();
                CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();
                CreateMap<Pelicula, PeliculaDto>().ReverseMap();
        }
    }
}
