﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.DTOS.PeliculaDTO
{
    public class PeliculaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la pelicula es obligatorio")]
        public string Nombre { get; set; }
        public byte[] RutaImagen { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La duracion es obligatoria")]
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }

        public TipoClasificacion Clasificacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int categoriaId { get; set; }
    }
}
