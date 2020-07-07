using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Chapter : VMBase.BaseModel<int>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionText { get; set; }

        public string AnswerText1 { get; set; }

        public string AnswerText2 { get; set; }

        public bool Answer { get; set; }

        public Image Image { get; set; }

        public int CourseId { get; set; }

        public static Chapter FromModel(ChapterModel model)
        {
            return model == null ? null : new Chapter()
            {
                Id = model.Id,
                Title = model.Title,
                Text = model.Text,
                Order = model.Order,
                QuestionText = model.QuestionText,
                QuestionTitle = model.QuestionTitle,
                AnswerText1 = model.AnswerText1,
                AnswerText2 = model.AnswerText2,
                Answer = model.Answer,
                Image = Image.FromModel(model.Image),
                CourseId = model.CourseId
            };
        }

        public ChapterModel ToModel()
        {
            return new ChapterModel()
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
                Image = this.Image.ToModel(),
                CourseId = this.CourseId
            };
        }
    }
}
