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

        public int? Frequency { get; set; }

        public string FrequencyPeriod { get; set; }

        public int? ParentId { get; set; }

        public Image Image { get; set; }

        public IList<int> Targets { get; set; }

        public IList<int> Diets { get; set; }

        public IList<int> ContraindicatedDiets { get; set; }

        public IList<int> Indications { get; set; }

        public IList<int> Contraindications { get; set; }

        public IList<int> Motivations { get; set; }

        public IList<int> ContraindicatedMotivations { get; set; }

        public static Method FromModel(MethodModel model)
        {
            return model == null ? null : new Method()
            {
                Id = model.Id,
                Type = model.Type,
                Title = model.Title,
                Text = model.Text,
                Frequency = model.Frequency,
                FrequencyPeriod = model.FrequencyPeriod,
                ParentId = model.ParentId,
                Image = model.Image == null ? null : Image.FromModel(model.Image),
                Targets = model.Targets?.ToList(),
                Diets = model.Diets?.ToList(),
                ContraindicatedDiets = model.ContraindicatedDiets?.ToList(),
                Indications = model.Indications?.ToList(),
                Contraindications = model.Contraindications?.ToList(),
                Motivations = model.Motivations?.ToList(),
                ContraindicatedMotivations = model.ContraindicatedMotivations?.ToList()
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
                Frequency = this.Frequency,
                FrequencyPeriod = this.FrequencyPeriod,
                ParentId = this.ParentId,
                Image = this.Image?.ToModel(),
                Targets = this.Targets?.ToList(),
                Diets = this.Diets?.ToList(),
                ContraindicatedDiets = this.ContraindicatedDiets?.ToList(),
                Indications = this.Indications?.ToList(),
                Contraindications = this.Contraindications?.ToList(),
                Motivations = this.Motivations?.ToList(),
                ContraindicatedMotivations = this.ContraindicatedMotivations?.ToList()
            };
        }
    }
}
