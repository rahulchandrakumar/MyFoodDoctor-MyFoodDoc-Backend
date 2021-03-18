using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Choice : VMBase.BaseModel<int>
    {
        public string Text { get; set; }

        public int Order { get; set; }

        public bool Scorable { get; set; }

        public int QuestionId { get; set; }

        public static Choice FromModel(ChoiceModel model)
        {
            return model == null ? null : new Choice()
            {
                Id = model.Id,
                Text = model.Text,
                Order = model.Order,
                Scorable = model.Scorable,
                QuestionId = model.QuestionId
            };
        }

        public ChoiceModel ToModel()
        {
            return new ChoiceModel()
            {
                Id = this.Id,
                Text = this.Text,
                Order = this.Order,
                Scorable = this.Scorable,
                QuestionId = this.QuestionId
            };
        }
    }
}