using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Actor;
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
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesStorage _filesStorage;
        private readonly string container = "actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFilesStorage filesStorage)
        {
            _context = context;
            _mapper = mapper;
            _filesStorage = filesStorage;
        }

        /// <summary>
        /// Get the paginated list of actors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> GetAsync([FromQuery] PaginatorDTO paginatorDTO)
        {
            var query = _context.Actors.AsQueryable();
            await HttpContext.InsertPaginationParameters(query, paginatorDTO.CountPerPage);
            var entities = await query.Paginate(paginatorDTO).ToListAsync();
            return _mapper.Map<List<ActorDTO>>(entities);
        }

        /// <summary>
        /// Get an actor details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetActor")]
        public async Task<ActionResult<ActorDTO>> GetAsync(int id)
        {
            var entity = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) { return NotFound(); }
            return _mapper.Map<ActorDTO>(entity);
        }

        /// <summary>
        /// Create a new actor
        /// </summary>
        /// <param name="actorCreationDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var entity = _mapper.Map<Actor>(actorCreationDTO);

            if (actorCreationDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreationDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreationDTO.Photo.FileName);
                    entity.Photo = await _filesStorage.SaveFile(content, extension, container, actorCreationDTO.Photo.ContentType);
                }
            }
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            var actorDTO = _mapper.Map<ActorDTO>(entity);

            return new CreatedAtRouteResult("GetActor", new { Id = actorDTO.Id }, actorDTO);
        }

        /// <summary>
        /// Update an actor data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actorEditionDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromForm] ActorEditionDTO actorEditionDTO)
        {
            var actorDB = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (actorDB == null) { return NotFound(); }

            actorDB = _mapper.Map(actorEditionDTO, actorDB);

            if (actorEditionDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorEditionDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorEditionDTO.Photo.FileName);
                    actorDB.Photo = await _filesStorage.EditFile(content, extension, container, actorDB.Photo, actorEditionDTO.Photo.ContentType);
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Update actor data partially
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonPatch"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> jsonPatch)
        {
            if(jsonPatch == null) { return BadRequest(); }
            var entityDB = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if(entityDB == null) { return NotFound(); }

            var entityDTO = _mapper.Map<ActorPatchDTO>(entityDB);
            jsonPatch.ApplyTo(entityDTO, ModelState);

            var isValidModel = TryValidateModel(entityDTO);
            if (!isValidModel) { return BadRequest(ModelState); }

            _mapper.Map(entityDTO, entityDB);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Delete an actor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var existst = await _context.Actors.AnyAsync(x => x.Id == id);

            if (!existst) { return NotFound(); }

            _context.Remove(new Actor() { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
