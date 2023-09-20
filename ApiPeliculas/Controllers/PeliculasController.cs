using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
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

        /// <summary>
        /// Obtenemos listado de peliculas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculass()
        {
            IEnumerable<Pelicula> listaPeliculas = _pelRepo.GetPeliculas().ToList();
         
            var listaPeliculaDto = new List<PeliculaDto>();

            foreach (var item in listaPeliculas)
            {
                listaPeliculaDto.Add(_mapper.Map<PeliculaDto>(item));
            }
            return Ok(listaPeliculaDto);
        }

        /// <summary>
        /// Obtenemos pelicula por su id
        /// </summary>
        /// <param name="peliculaId">id de la pelicula</param>
        /// <returns></returns>
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var peliculaInBBDD = _pelRepo.GetPelicula(peliculaId);

            //Comprobamos que la pelicula no es nula
            if (peliculaInBBDD == null)
            {
                return BadRequest($"No encontramos la pelicula para el id: {peliculaId}");
            }

            //Mapeamos la pelicula con mapper
            var itemPeliculaDto = _mapper.Map<PeliculaDto>(peliculaInBBDD);

            //devolvemos la pelicula
            return Ok(itemPeliculaDto);
        }
    }
}
