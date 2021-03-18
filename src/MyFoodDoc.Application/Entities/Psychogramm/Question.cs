using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.Application.Entities.Psychogramm
{
    public class Question : AbstractAuditableEntity
    {
        public string Text { get; set; }

        public int Order { get; set; }

        public bool VerticalAlignment { get; set; }

        public QuestionType Type { get; set; }

        public bool Extra { get; set; }

        public int ScaleId { get; set; }

        public ICollection<Choice> Choices { get; set; }

        public Scale Scale { get; set; }
    }
}
