using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.Psychogramm
{
    public class InsertChoicesPayload
    {
        public IEnumerable<QuestionPayload> Questions { get; set; }
    }
}
