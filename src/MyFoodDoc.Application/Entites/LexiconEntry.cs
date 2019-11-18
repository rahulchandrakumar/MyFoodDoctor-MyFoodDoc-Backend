using MyFoodDoc.Application.Abstractions;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entites
{
    public class LexiconEntry : AbstractAuditableEntity<int>
    {
        public string TitleShort { get; set; }

        public string TitleLong { get; set; }

        public string ImageUrl { get; set; }

        public string Text { get; set; }
    }
}