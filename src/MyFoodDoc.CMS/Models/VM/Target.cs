using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Target : VMBase.BaseModel<int>
    {
        public int OptimizationAreaId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string TriggerOperator { get; set; }
        public int TriggerValue { get; set; }
        public int Threshold { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public Image Image { get; set; }

        public static Target FromModel(TargetModel model)
        {
            return model == null ? null : new Target()
            {
                Id = model.Id,
                OptimizationAreaId = model.OptimizationAreaId,
                Title = model.Title,
                Text = model.Text,
                TriggerOperator = model.TriggerOperator,
                TriggerValue = model.TriggerValue,
                Threshold = model.Threshold,
                Priority = model.Priority,
                Type = model.Type,
                Image = Image.FromModel(model.Image)
            };
        }

        public TargetModel ToModel()
        {
            return new TargetModel()
            {
                Id = this.Id,
                OptimizationAreaId = this.OptimizationAreaId,
                Title = this.Title,
                Text = this.Text,
                TriggerOperator = this.TriggerOperator,
                TriggerValue = this.TriggerValue,
                Threshold = this.Threshold,
                Priority = this.Priority,
                Type = this.Type,
                Image = this.Image.ToModel()
            };
        }
    }
}
