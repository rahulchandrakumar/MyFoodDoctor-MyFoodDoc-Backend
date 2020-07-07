using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class MethodMultipleChoice : VMBase.BaseModel<int>
    {
        public string Title { get; set; }

        public bool IsCorrect { get; set; }

        public int MethodId { get; set; }

        public static MethodMultipleChoice FromModel(MethodMultipleChoiceModel model)
        {
            return model == null ? null : new MethodMultipleChoice()
            {
                Id = model.Id,
                Title = model.Title,
                IsCorrect = model.IsCorrect,
                MethodId = model.MethodId
            };
        }

        public MethodMultipleChoiceModel ToModel()
        {
            return new MethodMultipleChoiceModel()
            {
                Id = this.Id,
                Title = this.Title,
                IsCorrect = this.IsCorrect,
                MethodId = this.MethodId
            };
        }
    }
}
