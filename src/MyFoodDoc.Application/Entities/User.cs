﻿using Microsoft.AspNetCore.Identity;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.TrackedValues;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using MyFoodDoc.Application.Entities.Diary;
using MyFoodDoc.Application.Entities.Subscriptions;

namespace MyFoodDoc.Application.Entities
{
    public class User : IdentityUser, IAuditable
    {
        public DateTime? Birthday { get; set; }

        public Gender? Gender { get; set; }

        public decimal? Height { get; set; }

        public int? InsuranceId { get; set; }

        public bool PushNotificationsEnabled { get; set; }

        public string DeviceToken { get; set; }
        public bool HasZPPVersion { get; set; } = false;

        public Insurance Insurance { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }

        public ICollection<UserMotivation> Motivations { get; set; }

        public ICollection<UserIndication> Indications { get; set; }

        public ICollection<UserDiet> Diets { get; set; }

        public ICollection<UserWeight> WeightHistory { get; set; } = new List<UserWeight>();

        public ICollection<UserAbdominalGirth> AbdominalGirthHistory { get; set; } = new List<UserAbdominalGirth>();

        public ICollection<Meal> Meals { get; set; } = new List<Meal>();

        public ICollection<UserTarget> UserTargets { get; set; } = new List<UserTarget>();

        public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
        
        public ICollection<AppStoreSubscription> AppStoreSubscriptions { get; set; } = new List<AppStoreSubscription>();
        public ICollection<GooglePlayStoreSubscription> GooglePlayStoreSubscriptions { get; set; } = new List<GooglePlayStoreSubscription>();
    }
}
