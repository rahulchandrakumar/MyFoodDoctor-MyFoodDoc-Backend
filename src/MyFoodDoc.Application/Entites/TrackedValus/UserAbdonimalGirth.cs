using MyFoodDoc.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.TrackedValus
{
    public class UserAbdonimalGirth : AbstractTrackedValue<int>
    {
        public string UserId { get; set; }
    }
}
