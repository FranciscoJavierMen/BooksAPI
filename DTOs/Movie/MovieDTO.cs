using System;

namespace MoviesApi.DTOs.Movie
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
    }
}
