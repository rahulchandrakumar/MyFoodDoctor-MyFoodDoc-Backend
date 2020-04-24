using MyFoodDoc.Application.Entites.Targets;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Application.Enums;
using System;

namespace MyFoodDoc.CMS.Application.Models
{
    public class TargetModel : BaseModel<int>
    {
        public int OptimizationAreaId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string TriggerOperator { get; set; }
        public int TriggerValue { get; set; }
        public int Threshold { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public ImageModel Image { get; set; }
        public int ImageId { get; set; }

        public static TargetModel FromEntity(Target entity)
        {
            return entity == null ? null : new TargetModel()
            {
                Id = entity.Id,
                OptimizationAreaId = entity.OptimizationAreaId,
                Title = entity.Title,
                Text = entity.Text,
                TriggerOperator = entity.TriggerOperator.ToString(),
                TriggerValue = entity.TriggerValue,
                Threshold = entity.Threshold,
                Priority = entity.Priority.ToString(),
                Type = entity.Type.ToString(),
                Image = ImageModel.FromEntity(entity.Image),
            };
        }

        public Target ToEntity()
        {
            return new Target()
            {
                Id = this.Id,
                OptimizationAreaId = this.OptimizationAreaId,
                Title = this.Title,
                Text = this.Text,
                TriggerOperator = (TriggerOperator)Enum.Parse(typeof(TriggerOperator), this.TriggerOperator),
                TriggerValue = this.TriggerValue,
                Threshold = this.Threshold,
                Priority = (TargetPriority)Enum.Parse(typeof(TargetPriority), this.Priority),
                Type = (TargetType)Enum.Parse(typeof(TargetType), this.Type),
                ImageId = this.Image.Id,
            };
        }
    }
}
