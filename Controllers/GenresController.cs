using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public GenresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> GetAsync()
        {
            var entities = await _context.Genres.ToListAsync();
            var dtos = _mapper.Map<List<GenreDTO>>(entities);
            return dtos;
        }

        [HttpGet("{id}", Name = "GetGenre")]
        public async Task<ActionResult<GenreDTO>> GetAsync(int id)
        {
            var entity = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if(entity == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<GenreDTO>(entity);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] GenreCreationDTO createGenreDTO)
        {
            var entity = _mapper.Map<Genre>(createGenreDTO);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            var genreDTO = _mapper.Map<GenreDTO>(entity);

            return new CreatedAtRouteResult("GetGenre", new { id = genreDTO.Id }, genreDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] GenreEditionDTO genreEditionDTO)
        {
            var entity = _mapper.Map<Genre>(genreEditionDTO);
            entity.Id = id;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsybc(int id)
        {
            var exists = await _context.Genres.AnyAsync(x => x.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            _context.Remove(new Genre { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
