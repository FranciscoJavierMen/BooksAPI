using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;
using MoviesApi.Helpers;
using MoviesApi.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesStorage _filesStorage;
        private readonly string container = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFilesStorage filesStorage)
        {
            _context = context;
            _mapper = mapper;
            _filesStorage = filesStorage;
        }

        /// <summary>
        /// Get the paginated list of movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> GetAsync([FromQuery] PaginatorDTO paginatorDTO)
        {
            var query = _context.Movies.AsQueryable();
            await HttpContext.InsertPaginationParameters(query, paginatorDTO.CountPerPage);
            var movies = await query.Paginate(paginatorDTO).ToListAsync();
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        /// <summary>
        /// Get a movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetMovie")]
        public async Task<ActionResult<MovieDTO>> GetAsync(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if(movie == null) { return NotFound(); }

            return _mapper.Map<MovieDTO>(movie);
        }

        /// <summary>
        /// Create a new movie
        /// </summary>
        /// <param name="creationDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromForm] MovieCreationDTO creationDTO)
        {
            var movie = _mapper.Map<Movie>(creationDTO);

            if (creationDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await creationDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(creationDTO.Poster.FileName);
                    movie.Poster = await _filesStorage.SaveFile(content, extension, container, creationDTO.Poster.ContentType);
                }
            }

            AssignOrderActors(movie);
            _context.Add(movie);
            await _context.SaveChangesAsync();
            var movieDTO = _mapper.Map<MovieDTO>(movie);

            return new CreatedAtRouteResult("GetMovie", new { id = movieDTO.Id }, movieDTO);
        }

        private void AssignOrderActors(Movie movie)
        {
            if(movie.MoviesActors != null)
            {
                for(int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }

        /// <summary>
        /// Update a movie data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="editionDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromForm] MovieEditionDTO editionDTO)
        {
            var movie = await _context.Movies
                .Include(x => x.MoviesActors)
                .Include(x => x.MoviesGenres)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(movie == null) { return NotFound();  }

            var movieDTO = _mapper.Map(editionDTO, movie);
            if(editionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await editionDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(editionDTO.Poster.FileName);
                    movie.Poster = await _filesStorage.EditFile(content, extension, container, movie.Poster, editionDTO.Poster.ContentType);
                }
            }

            AssignOrderActors(movie);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Update a movie data partially
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonPatch"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> jsonPatch)
        {
            if (jsonPatch == null) { return BadRequest(); }
            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null) { return NotFound(); }

            var movieDTO = _mapper.Map<MoviePatchDTO>(movie);
            jsonPatch.ApplyTo(movieDTO, ModelState);

            var isValidModel = TryValidateModel(movieDTO);
            if (!isValidModel) { return BadRequest(ModelState); }

            _mapper.Map(movieDTO, movie);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var exists = await _context.Movies.AnyAsync(x => x.Id == id);

            if(!exists) { return NotFound();  }

            _context.Remove(new Movie() { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
