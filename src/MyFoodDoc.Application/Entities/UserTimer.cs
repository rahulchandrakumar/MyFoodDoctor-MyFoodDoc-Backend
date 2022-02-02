using MyFoodDoc.Application.Entities.Methods;
using System;

namespace MyFoodDoc.Application.Entities
{
    public class UserTimer
    {
        public string UserId { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int MethodId { get; set; }

        public User User { get; set; }

        public Method Method { get; set; }
    }
}
