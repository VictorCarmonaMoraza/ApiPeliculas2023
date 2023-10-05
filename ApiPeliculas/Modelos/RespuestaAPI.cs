using System.Net;

namespace ApiPeliculas.Modelos
{
    /// <summary>
    /// Modelo de ReespuestaAPI
    /// </summary>
    public class RespuestaAPI
    {
        public RespuestaAPI()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set;}
        public object Result { get; set; }
    }
}

//Esta clase sirve para capturar los errores del servidor