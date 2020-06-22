using MyFoodDoc.Application.Abstractions;
using System;

namespace MyFoodDoc.Application.Entities
{
    public class Image : AbstractAuditableEntity
    {
        public string Url { get; set; }
    }
}
