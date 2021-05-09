using Microsoft.AspNetCore.Http;
using MoviesApi.Validations;
using System;

namespace MoviesApi.DTOs.Movie
{
    public class MovieEditionDTO
    {
        public string Title { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime ReleaseDate { get; set; }
        [FileSizeValidation(4)]
        [FileTypeValidation(FileType.Image)]
        public IFormFile Poster { get; set; }
    }
}
