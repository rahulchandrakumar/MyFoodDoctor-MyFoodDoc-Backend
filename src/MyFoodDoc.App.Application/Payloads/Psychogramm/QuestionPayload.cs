using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.Psychogramm
{
    public class QuestionPayload
    {
        public int Id { get; set; }

        public IEnumerable<ChoicePayload> Choices { get; set; }
    }
}
