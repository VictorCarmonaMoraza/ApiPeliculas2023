using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    //Opcion 1 => [Route("api/[controller]")]
    //Opcion 2 =>[Route("api/categoria")] =>esta es mejor porque si cambia el nombre del controlador seguira funcionando
    [Route("api/categoria")]
    public class CategoriasController : ControllerBase
    {

        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        //Constructor
        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        #region GET

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            //Forma 1
            IEnumerable<Categoria> listaCategorias = _ctRepo.GetCategorias().ToList();
            //Forma 2
            var listaCategorias2 = _ctRepo.GetCategorias().ToList();

            //Forma 3
            var listaCategorias3 = from pelicula in _ctRepo.GetCategorias() select pelicula;

            //
            var listaCategoriaDto = new List<CategoriaDto>();

            foreach (var item in listaCategorias)
            {
                listaCategoriaDto.Add(_mapper.Map<CategoriaDto>(item));
            }
            return Ok(listaCategoriaDto);
        }

        /// <summary>
        /// Obtener cattegoria por su id
        /// </summary>
        /// <param name="categoriaId">Id de la categoria</param>
        /// <returns></returns>
        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
        //Forma 1
            Categoria itemCategoria = _ctRepo.GetCategoria(categoriaId);
            //Forma 2
            var itemCategorias2 = _ctRepo.GetCategoria(categoriaId);

            //Validamos si la categoria existe
            if (itemCategoria == null)
            {
                //Retorna un 400
                return NotFound();
            }

            //MApeamos
            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);

            return Ok(itemCategoriaDto);
        }

        #endregion GET

        #region POST

        [HttpPost]
        [ProducesResponseType(201,Type =typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            //Validamos el modelo, si el modelo no es valido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Validamos si el modelo viene vacio
            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }

            //Si existe una categoria con el mismo nombre, no nos debe dejar crear una categoria
            if (_ctRepo.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);

            //SIno podemos crear la categoria
            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal, guardando el registro{categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new {categoriaId= categoria.Id}, categoria);

        }

        #endregion POST

        #region PATCH

        [HttpPatch("{categoriaId:int}",Name = "ActualizarCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            //Validamos el modelo, si el modelo no es valido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Validamos si el modelo viene vacio
            if (categoriaDto == null || categoriaId !=categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }

            //Obtenemos la categoria
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            //SIno podemos crear la categoria
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro, guardando el registro{categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        #endregion PaTCH

    }
}


