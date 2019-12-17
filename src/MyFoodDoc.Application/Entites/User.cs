using Microsoft.AspNetCore.Identity;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.TrackedValus;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFoodDoc.Application.Entites
{
    public class User : IdentityUser, IAuditable
    {
        public DateTime? Birthday { get; set; }

        public Gender? Gender { get; set; }

        public decimal? Height { get; set; }

        public int? InsuranceId { get; set; }

        public Insurance Insurance { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }

        public ICollection<UserMotivation> Motivations { get; set; }

        public ICollection<UserIndication> Indications { get; set; }

        public ICollection<UserDiet> Diets { get; set; }

        public ICollection<UserWeight> WeightHistory { get; set; } = new List<UserWeight>();

        public ICollection<UserAbdominalGirth> AbdominalGirthHistory { get; set; } = new List<UserAbdominalGirth>();
    }
}
