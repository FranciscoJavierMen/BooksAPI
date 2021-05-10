using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Actor;
using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;
using System.Collections.Generic;

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
            CreateMap<MovieCreationDTO, Movie>().ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenre))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMovieActors));
            CreateMap<MovieEditionDTO, Movie>().ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<MoviePatchDTO, Movie>().ReverseMap();
        }

        private List<MoviesGenres> MapMoviesGenre(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();
            if(movieCreationDTO.GenresIds == null) { return result; }

            foreach(var id in movieCreationDTO.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }

            return result;
        }

        private List<MoviesActors> MapMovieActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActors>();
            if (movieCreationDTO.GenresIds == null) { return result; }

            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new MoviesActors() { ActorId = actor.ActorId, Character = actor.Character });
            }

            return result;
        }
    }
}
