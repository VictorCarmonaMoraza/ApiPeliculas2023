using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    //Opcion 1 => [Route("api/[controller]")]
    //Opcion 2 =>[Route("api/categoria")] =>esta es mejor porque si cambia el nombre del controlador seguira funcionando
    [Route("api/categoria")]
    public class CategoriasController : ControllerBase
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
