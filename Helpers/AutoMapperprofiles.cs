using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Actor;
using MoviesApi.Entities;

namespace MoviesApi.Helpers
{
    public class AutoMapperprofiles: Profile
    {
        public AutoMapperprofiles()
        {
            //Map for genre
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
            CreateMap<GenreEditionDTO, Genre>();
            //Map for actors
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>().ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<ActorEditionDTO, Actor>().ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();
        }
    }
}
