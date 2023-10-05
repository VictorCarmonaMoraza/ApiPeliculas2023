using ApiPeliculas.DTOS.CategoriaDTO;
using ApiPeliculas.DTOS.UsuarioDTO;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiPeliculas.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : Controller
    {

        private readonly IUsuarioRepositorio _usRepo;
        private readonly IMapper _mapper;
        protected RespuestaAPI _respuestaAPI;


        /// <summary>
        /// Coonstructor
        /// </summary>
        /// <param name="usRepo"></param>
        /// <param name="mapper"></param>
        public UsuariosController(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            this._respuestaAPI = new();
            _mapper = mapper;
        }

        #region GET
        /// <summary>
        /// Obtiene la lista de usuarios
        /// </summary>
        /// <returns>lista de usuarios</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuarios()
        {
            //Obtenemos la lista de usuarios de la base de datos
            var listaUsuarios = _usRepo.GetUsuarios().ToList();

            if(listaUsuarios == null || listaUsuarios.Count == 0)
            {
                return BadRequest("No tenemos usuarios en la base de datos");
            }
            
            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var item in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(item));
            }
            return Ok(listaUsuariosDto);
        }

        /// <summary>
        /// Obtiene un usuario por su id
        /// </summary>
        /// <param name="usuarioId">id del usuario</param>
        /// <returns>usuario</returns>
        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int usuarioId)
        {
            Usuario itemUsuario = _usRepo.GetUsuario(usuarioId);

            //Validamos si el usuaerio exisate
            if (itemUsuario == null)
            {
                //Retorna un 400
                return NotFound();
            }

            //MApeamos
            var itemUsuarioiaDto = _mapper.Map<UsuarioDto>(itemUsuario);

            return Ok(itemUsuarioiaDto);
        }

        #endregion GET

        #region POST
        /// <summary>
        /// Registro de Usuario
        /// </summary>
        /// <param name="usuarioRegistroDto">usuario a registrar</param>
        /// <returns></returns>
        [HttpPost("registro")]
        [ProducesResponseType(201, Type = typeof(UsuarioDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            //Validamos si el ususario es unico
            bool nombreUsuarioEsUnico = _usRepo.IsUniqueUser(usuarioRegistroDto.NombreUsuario);

            //Si el usuario ya existe
            if (!nombreUsuarioEsUnico)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre del usuario ya existe");
                return BadRequest(_respuestaAPI);
            }

            //Creamos usuario
            var usuario = await _usRepo.Registro(usuarioRegistroDto);

            if (usuario == null)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("Error en el registro");
                return BadRequest(_respuestaAPI);
            }

            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.IsSuccess = true;
            return Ok(_respuestaAPI);
        }

        #endregion POST
    }
}
