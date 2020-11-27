using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class MethodText : VMBase.BaseModel<int>
    {
        public int MethodId { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public static MethodText FromModel(MethodTextModel model)
        {
            return model == null ? null : new MethodText()
            {
                Id = model.Id,
                Code = model.Code,
                Title = model.Title,
                Text = model.Text,
                MethodId = model.MethodId
            };
        }

        public MethodTextModel ToModel()
        {
            return new MethodTextModel()
            {
                Id = this.Id,
                Code = this.Code,
                Title = this.Title,
                Text = this.Text,
                MethodId = this.MethodId
            };
        }

    }
}
