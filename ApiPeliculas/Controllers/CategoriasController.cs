using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

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


    }
}
