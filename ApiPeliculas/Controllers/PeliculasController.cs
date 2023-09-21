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
        #region campos
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;
        #endregion campos

        #region constructor
        //Constructor
        public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
        }
        #endregion constructor

        #region GET
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
        #endregion GET

        #region POST
        /// <summary>
        /// Creacion de una pelicula
        /// </summary>
        /// <param name="peliculaDto">modelo pelicula</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPelicula([FromBody] PeliculaDto peliculaDto)
        {
            //Validamos el modelo, si el modelo no es valido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Validamos si el modelo viene vacio
            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            //Si existe una categoria con el mismo nombre, no nos debe dejar crear una categoria
            if (_pelRepo.ExistePelicula(peliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La pelicula ya existe");
                return StatusCode(404, ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            //SIno podemos crear la categoria
            if (!_pelRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal, guardando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);

        }

        #endregion POST

        #region PATCH
        /// <summary>
        /// Actualiza una pelicula
        /// </summary>
        /// <param name="peliculaId"></param>
        /// <param name="peliculaDto"></param>
        /// <returns></returns>
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            //Validamos el modelo, si el modelo no es valido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Validamos si el modelo viene vacio
            if (peliculaDto == null || peliculaId != peliculaDto.Id)
            {
                return BadRequest(ModelState);
            }

            //Obtenemos la categoria
            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            //SIno podemos crear la categoria
            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro, guardando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        #endregion PATCH
    }
}
