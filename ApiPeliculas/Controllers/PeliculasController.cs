using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ApiPeliculas.DTOS.CategoriaDTO;
using ApiPeliculas.DTOS.PeliculaDTO;
using Microsoft.AspNetCore.Authorization;

namespace ApiPeliculas.Controllers
{

    [Route("api/Peliculas")]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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


        /// <summary>
        /// Obtener peliculas dentro de una categoria
        /// </summary>
        /// <param name="categoriaId">id categoria</param>
        /// <returns>Lista de peliculas dentro de la categoria</returns>
        [AllowAnonymous]
        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);

            if (listaPeliculas == null)
            {
                return BadRequest($"No encontramos ninguna pelicula para el id: {categoriaId}");
            }

            var itemPelicula = new List<PeliculaDto>();

            foreach (var item in listaPeliculas)
            {
                itemPelicula.Add(_mapper.Map<PeliculaDto>(item));
            }
            return Ok(itemPelicula);

        }

        /// <summary>
        /// Buscar una pelicula por su nombre
        /// </summary>
        /// <param name="nombre">nombre de la pelicula</param>
        /// <returns>pelicula encontrada</returns>
        [AllowAnonymous]
        [HttpGet("Buscar")]
        public IActionResult Buscar(string nombre)
        {

            try
            {
                var resultado = _pelRepo.BuscarPelicula(nombre.Trim());
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }
        #endregion GET

        #region POST
        /// <summary>
        /// Creacion de una pelicula
        /// </summary>
        /// <param name="peliculaDto">modelo pelicula</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [Authorize(Roles = "admin")]
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        #region DELETE
        /// <summary>
        /// Elimina una pelicula por su id
        /// </summary>
        /// <param name="peliculaId">id d ela pelicula</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //// [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarPelicula(int peliculaId)
        {
            //Si no existe la pelicula
            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return BadRequest($"No encontramos la pelicula para el id: {peliculaId}");
            }

            var pelicula = _pelRepo.GetPelicula(peliculaId);

            //Borramos la pelicula
            if (!_pelRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando la pelicula {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        #endregion DELETE
    }
}
