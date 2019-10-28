using System;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class DiaryLiquid : AbstractAuditEntity<int>
    {
        public int Amount { get; set; }

        public DateTime LastAdded { get; set; }
    }
}