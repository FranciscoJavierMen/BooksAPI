﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(140)]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/YYYY}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        public string Photo { get; set; }
        public List<MoviesActors> MoviesActors{ get; set; }
    }
}
