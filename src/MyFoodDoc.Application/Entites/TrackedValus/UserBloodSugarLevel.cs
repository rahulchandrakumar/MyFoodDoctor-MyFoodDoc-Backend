using MyFoodDoc.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.TrackedValus
{
    public class UserBloodSugarLevel : AbstractTrackedValue<int>
    {
        public string UserId { get; set; }
    }
}
