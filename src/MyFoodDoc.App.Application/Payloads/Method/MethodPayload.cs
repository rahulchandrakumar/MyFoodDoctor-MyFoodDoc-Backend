using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Method
{
    public class MethodPayload
    {
        public int Id { get; set; }

        public bool? UserAnswerBoolean { get; set; }

        public decimal? UserAnswerDecimal { get; set; }

        public int? UserAnswerInteger { get; set; }

        public IEnumerable<MethodMultipleChoicePayload> Choices { get; set; }
    }
}
