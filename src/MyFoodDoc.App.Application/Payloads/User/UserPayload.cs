using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class UserPayload
    {
        public string Birthday { get; set; }

        public Gender Gender { get; set; }

        public int Height { get; set; }

        public int InsuranceId { get; set; }

        public ICollection<string> Indications { get; set; }

        public ICollection<string> Motivations { get; set; }

        public ICollection<string> Diet { get; set; }
    }
}
