using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.ActorMovies;
using MoviesApi.Helpers;
using MoviesApi.Validations;
using System;
using System.Collections.Generic;

namespace MoviesApi.DTOs.Movie
{
    public class MovieCreationDTO
    {
        public string Title { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime ReleaseDate { get; set; }
        [FileSizeValidation(4)]
        [FileTypeValidation(FileType.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorMoviesCreationDTO>>))]
        public List<ActorMoviesCreationDTO> Actors{ get; set; }
    }
}
