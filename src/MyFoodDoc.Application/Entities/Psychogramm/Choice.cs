using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities.Psychogramm
{
    public class Choice : AbstractAuditableEntity
    {
        public string Text { get; set; }

        public int Order { get; set; }

        public bool Scorable { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
