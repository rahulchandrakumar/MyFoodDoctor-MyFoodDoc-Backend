using MyFoodDoc.Application.Abstractions;
using System;

namespace MyFoodDoc.Application.Entites
{
    public class Image : AbstractAuditableEntity
    {
        public string Url { get; set; }
    }
}
