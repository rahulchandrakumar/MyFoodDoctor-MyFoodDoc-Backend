using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities
{
    public class LexiconCategory : AbstractAuditableEntity
    {
        public string Title { get; set; }

        public int ImageId { get; set; }

        public ICollection<LexiconEntry> Entries { get; set; }

        public Image Image { get; set; }
    }
}
