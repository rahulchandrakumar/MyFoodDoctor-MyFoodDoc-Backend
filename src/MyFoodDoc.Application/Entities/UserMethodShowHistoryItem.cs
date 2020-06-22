using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Abstractions;

namespace MyFoodDoc.Application.Entities
{
    public class UserMethodShowHistoryItem : AbstractEntity
    {
        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public int MethodId { get; set; }

        public User User { get; set; }

        public Method Method { get; set; }
    }
}
