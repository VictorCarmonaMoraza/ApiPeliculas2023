using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }


        //public int Id
        //{
        //    get { return IdCategoria; }
        //    set { IdCategoria = value; }
        //}
    }
}
