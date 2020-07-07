using MyFoodDoc.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities.Courses
{
    public class Chapter : AbstractAuditableEntity
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public int ImageId { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionText { get; set; }

        public string AnswerText1 { get; set; }

        public string AnswerText2 { get; set; }

        public bool Answer { get; set; }

        public int CourseId { get; set; }

        public ICollection<Subchapter> Subchapters { get; set; }

        public Image Image { get; set; }

        public Course Course { get; set; }
    }
}
