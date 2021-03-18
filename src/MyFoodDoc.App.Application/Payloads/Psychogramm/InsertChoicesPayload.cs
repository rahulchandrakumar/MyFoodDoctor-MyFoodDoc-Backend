using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Psychogramm
{
    public class InsertChoicesPayload
    {
        public IEnumerable<QuestionPayload> Questions { get; set; }
    }
}
