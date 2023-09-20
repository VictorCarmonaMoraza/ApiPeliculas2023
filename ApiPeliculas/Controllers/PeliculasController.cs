using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;

        //Constructor
        public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculass()
        {
            //Forma 1
            IEnumerable<Pelicula> listaPeliculas = _pelRepo.GetPeliculas().ToList();
            //Forma 2
            //var listaPeliculas2 = _pelRepo.GetPeliculas().ToList();

            //Forma 3
            //var listaPeliculas3 = from pelicula in _pelRepo.GetPeliculas() select pelicula;

            //
            var listaPeliculaDto = new List<PeliculaDto>();

            foreach (var item in listaPeliculas)
            {
                listaPeliculaDto.Add(_mapper.Map<PeliculaDto>(item));
            }
            return Ok(listaPeliculaDto);
        }
    }
}
