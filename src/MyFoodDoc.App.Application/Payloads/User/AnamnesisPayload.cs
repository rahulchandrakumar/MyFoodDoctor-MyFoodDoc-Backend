using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Enums;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class AnamnesisPayload
    {
        public Gender Gender { get; set; }

        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public ICollection<string> Motivations { get; set; }

        public ICollection<string> Indications { get; set; }

        public ICollection<string> Diets { get; set; }
    }
}
