using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Models
{
    public class ChapterDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<SubchapterDto> Subchapters { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionText { get; set; }

        public string AnswerText1 { get; set; }

        public string AnswerText2 { get; set; }

        public bool Answer { get; set; }

        public bool? UserAnswer { get; set; }
    }
}
