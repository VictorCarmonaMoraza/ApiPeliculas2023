using ApiPeliculas.Data;
using ApiPeliculas.DTOS.UsuarioDTO;
using ApiPeliculas.Helpers;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiPeliculas.Repositorio
{
    /// <summary>
    /// Repositorio de Usuario
    /// </summary>
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        private readonly ApplicationDbContext _bd;
        private string claveSecreta;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bd"></param>
        public UsuarioRepositorio(ApplicationDbContext bd, IConfiguration config,
            UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtenemos los usuario por su id
        /// </summary>
        /// <param name="usuarioId">id del usaurio</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public AppUsuario GetUsuario(string usuarioId)
        {
            return _bd.AppUsuario.FirstOrDefault(u=>u.Id == usuarioId);
        }

        /// <summary>
        /// Obtenemos lista de usaurios de la base de datos ordenados por el nombre
        /// </summary>
        /// <returns></returns>
        public ICollection<AppUsuario> GetUsuarios()
        {
            //Retornamos el nombre de usaurios ordenados por su nombre
            return _bd.AppUsuario.OrderBy(u => u.UserName).ToList();
        }

        /// <summary>
        /// Comprobar si el usaurio es unico
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool IsUniqueUser(string usuario)
        {
            var usuariobd = _bd.AppUsuario.FirstOrDefault(u=>u.UserName == usuario);

            //Comporbamos si el usaurio de la base de datos es igual a null
            if (usuariobd == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Registramos un Usuario
        /// </summary>
        /// <param name="usuarioRegistroDto"></param>
        /// <returns></returns>
        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            //var passwwordEncriptada = EncriptarPassword.obtenermd5(usuarioRegistroDto.Password);

            AppUsuario usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                //Password = passwwordEncriptada,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
                Nombre = usuarioRegistroDto.Nombre,
            };

            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);

            if(result.Succeeded)
            {
                //Si no existe Role. Solo la primera vez y es para crear los roles
                if(!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }

                //await _userManager.AddToRoleAsync(usuario, "admin");
                await _userManager.AddToRoleAsync(usuario, "registrado");
                var usuarioRetornado = _bd.AppUsuario.FirstOrDefault(u => u.UserName == usuarioRegistroDto.NombreUsuario);

                //Opcion 1 
                //return new UsuarioDatosDto()
                //{
                //    Id = usuarioRetornado.Id,
                //    UserName = usuarioRetornado.UserName,
                //    Nombre = usuarioRetornado.Nombre,
                //};

                //Opcion 2
                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
            }
            //Guardado en BBDD
            //_bd.Usuarios.Add(usuario);
            //await _bd.SaveChangesAsync();
            //usuario.Password = passwwordEncriptada;
            //return usuario;
            return new UsuarioDatosDto();
        }

        /// <summary>
        /// Login mediante token para el Usuario
        /// </summary>
        /// <param name="usuarioLoginDto"></param>
        /// <returns></returns>
        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            //var passwordEncriptado = EncriptarPassword.obtenermd5(usuarioLoginDto.Password);

            //Validamos soi el usuario no existe xcon la combinacion de usuario y contraseña correcta
            var usuario = _bd.AppUsuario.FirstOrDefault(
                u=>u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                //&& u.Password==passwordEncriptado
                );
            bool isValida = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);
            //validamos si el usaurio es nulo
            if(usuario == null || isValida == false)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //Aqui existe el usaurio entonces podemos procesar el login
            var  roles = await _userManager.GetRolesAsync(usuario);

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario)
            };

            return usuarioLoginRespuestaDto;
        }
    }
}
