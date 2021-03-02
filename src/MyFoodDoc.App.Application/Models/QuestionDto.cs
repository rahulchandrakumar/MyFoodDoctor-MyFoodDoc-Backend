using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public bool VerticalAlignment { get; set; }

        public ICollection<ChoiceDto> Choices { get; set; }
    }
}
