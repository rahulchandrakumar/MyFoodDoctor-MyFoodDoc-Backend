using System.Collections.Generic;
using System.Linq;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Method : VMBase.BaseModel<int>
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public Image Image { get; set; }

        public IList<int> Targets { get; set; }

        public IList<int> Diets { get; set; }

        public IList<int> Indications { get; set; }

        public IList<int> Motivations { get; set; }

        public static Method FromModel(MethodModel model)
        {
            return model == null ? null : new Method()
            {
                Id = model.Id,
                Type = model.Type,
                Title = model.Title,
                Text = model.Text,
                Image = model.Image == null ? null : Image.FromModel(model.Image),
                Targets = model.Targets?.ToList(),
                Diets = model.Diets?.ToList(),
                Indications = model.Indications?.ToList(),
                Motivations = model.Motivations?.ToList()
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
                Image = this.Image?.ToModel(),
                Targets = this.Targets?.ToList(),
                Diets = this.Diets?.ToList(),
                Indications = this.Indications?.ToList(),
                Motivations = this.Motivations?.ToList()
            };
        }
    }
}
