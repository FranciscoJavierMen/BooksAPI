using Microsoft.AspNetCore.Http;
using MoviesApi.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Actor
{
    public class ActorEditionDTO
    {
        [Required]
        [StringLength(140)]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/YYYY}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        [FileSizeValidation(4)]
        [FileTypeValidation(FileType.Image)]
        public IFormFile Photo { get; set; }
    }
}
