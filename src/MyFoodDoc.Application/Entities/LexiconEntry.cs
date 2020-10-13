using MyFoodDoc.Application.Abstractions;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entities
{
    public class LexiconEntry : AbstractAuditableEntity
    {
        public string TitleShort { get; set; }

        public string TitleLong { get; set; }

        public string Text { get; set; }

        public int ImageId { get; set; }

        public int CategoryId { get; set; }

        public Image Image { get; set; }

        public LexiconCategory Category { get; set; }
    }
}