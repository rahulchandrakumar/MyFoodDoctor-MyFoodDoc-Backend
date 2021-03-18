using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class ScaleDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public string ImageUrl { get; set; }

        public int CompletedQuestionsCount { get; set; }

        public int QuestionsCount { get; set; }

        public ICollection<QuestionDto> Questions { get; set; }
    }
}
