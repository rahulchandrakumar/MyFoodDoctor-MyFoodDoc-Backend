using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities.Psychogramm
{
    public class Scale : AbstractAuditableEntity
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public string TypeCode { get; set; }

        public string TypeTitle { get; set; }

        public string TypeText { get; set; }

        public string Characterization { get; set; }

        public string Reason { get; set; }

        public string Treatment { get; set; }

        public int ImageId { get; set; }

        public ICollection<Question> Questions { get; set; }

        public Image Image { get; set; }
    }
}
