using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Actor;
using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;

namespace MoviesApi.Helpers
{
    public class AutoMapperprofiles: Profile
    {
        public AutoMapperprofiles()
        {
            //Mapping for genre
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
            CreateMap<GenreEditionDTO, Genre>();
            //Mapping for actors
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>().ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<ActorEditionDTO, Actor>().ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();
            //Mapping for movies
            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieCreationDTO, Movie>().ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<MovieEditionDTO, Movie>().ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<MoviePatchDTO, Movie>().ReverseMap();
        }
    }
}
