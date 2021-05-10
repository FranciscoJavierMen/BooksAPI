using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Actor
{
    public class ActorPatchDTO
    {
        [Required]
        [StringLength(140)]
        public string Name { get; set; }
    }
}
