using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public byte[] RutaImagen { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho}

        public TipoClasificacion Clasificacion {  get; set; }

        public DateTime FechaCreacion { get; set; }

        //Campo para la relacion
        [ForeignKey("categoriaId")]
        public int categoriaId {  get; set; }

        public Categoria Categoria { get; set; }    



        //public int IdPelicula
        //{
        //    get { return Id; }
        //    set { Id = value; }
        //}

        //public string NombrePelicula
        //{
        //    get { return Nombre; }
        //    set { Nombre = value; }
        //}

        //public string RutaImagenPelicula
        //{
        //    get { return RutaImagen; }
        //    set { RutaImagen = value;}
        //}

        //public string DescripcionPelicula
        //{
        //    get { return Descripcion; }
        //    set { Descripcion = value; }
        //}

        //public int DuracionPelicula
        //{
        //    get { return Duracion; }
        //    set { Duracion = value; }
        //}

        //public DateTime FechaCreacionnPelicula
        //{
        //    get { return FechaCreacion; }
        //    set { FechaCreacion = value; }
        //}

    }
}
