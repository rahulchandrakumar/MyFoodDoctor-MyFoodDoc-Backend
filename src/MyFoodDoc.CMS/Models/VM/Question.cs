using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Question : VMBase.BaseModel<int>
    {
        public string Text { get; set; }

        public int Order { get; set; }

        public bool VerticalAlignment { get; set; }

        public string Type { get; set; }

        public bool Extra { get; set; }

        public int ScaleId { get; set; }

        public static Question FromModel(QuestionModel model)
        {
            return model == null ? null : new Question()
            {
                Id = model.Id,
                Text = model.Text,
                Order = model.Order,
                VerticalAlignment = model.VerticalAlignment,
                Type = model.Type,
                Extra = model.Extra,
                ScaleId = model.ScaleId
            };
        }

        public QuestionModel ToModel()
        {
            return new QuestionModel()
            {
                Id = this.Id,
                Text = this.Text,
                Order = this.Order,
                VerticalAlignment = this.VerticalAlignment,
                Type = this.Type,
                Extra = this.Extra,
                ScaleId = this.ScaleId
            };
        }
    }
}
