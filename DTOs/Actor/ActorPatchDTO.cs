using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Actor
{
    public class ActorPatchDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(140)]
        public string Name { get; set; }
    }
}
