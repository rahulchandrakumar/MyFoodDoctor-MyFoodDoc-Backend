using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Scale : VMBase.BaseModel<int>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public Image Image { get; set; }

        public static Scale FromModel(ScaleModel model)
        {
            return model == null ? null : new Scale()
            {
                Id = model.Id,
                Title = model.Title,
                Text = model.Text,
                Order = model.Order,
                Image = Image.FromModel(model.Image)
            };
        }

        public ScaleModel ToModel()
        {
            return new ScaleModel()
            {
                Id = this.Id,
                Title = this.Title,
                Text = this.Text,
                Order = this.Order,
                Image = this.Image.ToModel()
            };
        }
    }
}
