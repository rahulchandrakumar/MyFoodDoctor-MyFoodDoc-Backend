using System;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Liquid : AbstractAuditableEntity<int>
    {
        public int Amount { get; set; }

        public DateTime LastAdded { get; set; }
    }
}