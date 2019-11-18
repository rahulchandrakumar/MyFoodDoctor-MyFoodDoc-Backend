using System;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Exercise : AbstractAuditEntity<int>
    {
        public int Duration { get; set; }

        public DateTime LastAdded { get; set; }
    }
}