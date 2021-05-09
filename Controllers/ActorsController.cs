using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.DTOs.Actor;
using MoviesApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            var actorDTO = _mapper.Map<ActorDTO>(entity);

            return new CreatedAtRouteResult("GetActor", new { Id = actorDTO.Id }, actorDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromForm] ActorEditionDTO actorEditionDTO)
        {
            var entity = _mapper.Map<Actor>(actorEditionDTO);
            entity.Id = id;

            _context.Entry(entity).State = EntityState.Modified;
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
