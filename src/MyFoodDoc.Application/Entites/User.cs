using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyFoodDoc.Application.Entites
{
    public class User
    {
        public int Id { get; set; }

        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }

        public int? Height { get; set; }

        public Insurance Insurance { get; set; }

        public ICollection<Motivation> Motivations { get; set; }

        public ICollection<Indication> Indications { get; set; }

        public ICollection<Diet> Diets { get; set; }
    }
}
