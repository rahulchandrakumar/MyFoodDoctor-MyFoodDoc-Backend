using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class TargetDto
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string UserAnswerCode { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<TargetAnswerDto> Answers { get; set; }
    }
}
