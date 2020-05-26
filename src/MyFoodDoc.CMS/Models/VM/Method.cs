using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Method : VMBase.BaseModel<int>
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int TargetId { get; set; }

        public static Method FromModel(MethodModel model)
        {
            return model == null ? null : new Method()
            {
                Id = model.Id,
                Type = model.Type,
                Title = model.Title,
                Text = model.Text,
                TargetId = model.TargetId
            };
        }

        public MethodModel ToModel()
        {
            return new MethodModel()
            {
                Id = this.Id,
                Type = this.Type,
                Title = this.Title,
                Text = this.Text,
                TargetId = this.TargetId
            };
        }
    }
}
