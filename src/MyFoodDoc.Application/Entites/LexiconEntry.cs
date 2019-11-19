using MyFoodDoc.Application.Abstractions;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entites
{
    public class LexiconEntry : AbstractAuditableEntity
    {
        public string TitleShort { get; set; }

        public string TitleLong { get; set; }

        public int ImageId { get; set; }

        public string Text { get; set; }

        public Image Image { get; set; }
    }
}