using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Entites.Courses;

namespace MyFoodDoc.CMS.Application.Models
{
    public class ChapterModel : BaseModel<int>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionText { get; set; }

        public string AnswerText1 { get; set; }

        public string AnswerText2 { get; set; }

        public bool Answer { get; set; }

        public ImageModel Image { get; set; }

        public int CourseId { get; set; }

        public static ChapterModel FromEntity(Chapter entity)
        {
            return entity == null ? null : new ChapterModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Text = entity.Text,
                Order = entity.Order,
                QuestionText = entity.QuestionText,
                QuestionTitle = entity.QuestionTitle,
                AnswerText1 = entity.AnswerText1,
                AnswerText2 = entity.AnswerText2,
                Answer = entity.Answer,
                Image = entity.Image == null ? null : ImageModel.FromEntity(entity.Image),
                CourseId = entity.CourseId
            };
        }

        public Chapter ToEntity()
        {
            return new Chapter()
            {
                Id = this.Id,
                Title = this.Title,
                Text = this.Text,
                Order = this.Order,
                QuestionText = this.QuestionText,
                QuestionTitle = this.QuestionTitle,
                AnswerText1 = this.AnswerText1,
                AnswerText2 = this.AnswerText2,
                Answer = this.Answer,
                ImageId = this.Image == null || string.IsNullOrEmpty(this.Image.Url) ? (int?)null : this.Image.Id,
                CourseId = this.CourseId
            };
        }
    }
}
