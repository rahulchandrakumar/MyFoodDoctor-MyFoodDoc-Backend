using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities
{
    public class UserTimer
    {
        public string UserId { get; set; }

        public DateTime ExpirationDate { get; set; }

        public User User { get; set; }
    }
}
