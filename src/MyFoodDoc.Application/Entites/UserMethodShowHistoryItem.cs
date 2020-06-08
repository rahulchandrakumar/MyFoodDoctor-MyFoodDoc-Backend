using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Abstractions;

namespace MyFoodDoc.Application.Entites
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
