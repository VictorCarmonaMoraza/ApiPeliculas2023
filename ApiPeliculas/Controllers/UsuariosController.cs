﻿using ApiPeliculas.DTOS.CategoriaDTO;
using ApiPeliculas.DTOS.UsuarioDTO;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : Controller
    {

        private readonly IUsuarioRepositorio _usRepo;
        private readonly IMapper _mapper;


        /// <summary>
        /// Coonstructor
        /// </summary>
        /// <param name="usRepo"></param>
        /// <param name="mapper"></param>
        public UsuariosController(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            _mapper = mapper;
        }

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

    }
}