using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Movie
{
    public class MoviePatchDTO
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
