using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Scale : VMBase.BaseModel<int>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public string TypeCode { get; set; }

        public string TypeTitle { get; set; }

        public string TypeText { get; set; }

        public string Characterization { get; set; }

        public string Reason { get; set; }

        public string Treatment { get; set; }

        public Image Image { get; set; }

        public static Scale FromModel(ScaleModel model)
        {
            return model == null ? null : new Scale()
            {
                Id = model.Id,
                Title = model.Title,
                Text = model.Text,
                Order = model.Order,
                TypeCode = model.TypeCode,
                TypeTitle = model.TypeTitle,
                TypeText = model.TypeText,
                Characterization = model.Characterization,
                Reason = model.Reason,
                Treatment = model.Treatment,
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
                TypeCode = this.TypeCode,
                TypeTitle = this.TypeTitle,
                TypeText = this.TypeText,
                Characterization = this.Characterization,
                Reason = this.Reason,
                Treatment = this.Treatment,
                Image = this.Image.ToModel()
            };
        }
    }
}
