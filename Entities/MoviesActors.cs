﻿namespace MoviesApi.Entities
{
    public class MoviesActors
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string Character { get; set; }
        public int Order{ get; set; }
        public Actor Actor { get; set; }
        public Movie Movie { get; set; }
    }
}
