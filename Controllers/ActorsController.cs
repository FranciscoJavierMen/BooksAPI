using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.DTOs.Actor;
using MoviesApi.Entities;
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

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> GetAsync()
        {
            var entities = await _context.Actors.ToListAsync();
            return _mapper.Map<List<ActorDTO>>(entities);
        }

        [HttpGet("{id}", Name ="GetActor")]
        public async Task<ActionResult<ActorDTO>> GetAsync(int id)
        {
            var entity = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if(entity == null){ return NotFound(); }
            return _mapper.Map<ActorDTO>(entity);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var entity = _mapper.Map<Actor>(actorCreationDTO);

            if(actorCreationDTO.Photo != null)
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

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromForm] ActorEditionDTO actorEditionDTO)
        {
            var actorDB = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if(actorDB == null) { return NotFound(); }

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
