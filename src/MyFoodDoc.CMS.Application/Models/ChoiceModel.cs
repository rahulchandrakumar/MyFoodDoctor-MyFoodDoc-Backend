using MyFoodDoc.Application.Entities.Psychogramm;

namespace MyFoodDoc.CMS.Application.Models
{
    public class ChoiceModel : BaseModel<int>
    {
        public string Text { get; set; }

        public int Order { get; set; }

        public bool Scorable { get; set; }

        public int QuestionId { get; set; }

        public static ChoiceModel FromEntity(Choice entity)
        {
            return entity == null ? null : new ChoiceModel()
            {
                Id = entity.Id,
                Text = entity.Text,
                Order = entity.Order,
                Scorable = entity.Scorable,
                QuestionId = entity.QuestionId
            };
        }

        public Choice ToEntity()
        {
            return new Choice()
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
