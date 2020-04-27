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

        #region Adjustment target
        public int AdjustmentTargetId { get; set; }
        public int TargetValue { get; set; }
        public int Step { get; set; }
        public string StepDirection { get; set; }
        public string RecommendedText { get; set; }
        public string TargetText { get; set; }
        public string RemainText { get; set; }
        #endregion

        public ImageModel Image { get; set; }
        public int ImageId { get; set; }

        public static TargetModel FromEntity(Target target, AdjustmentTarget adjustmentTarget)
        {
            if (target == null)
                return null;

            var result = new TargetModel()
            {
                Id = target.Id,
                OptimizationAreaId = target.OptimizationAreaId,
                Title = target.Title,
                Text = target.Text,
                TriggerOperator = target.TriggerOperator.ToString(),
                TriggerValue = target.TriggerValue,
                Threshold = target.Threshold,
                Priority = target.Priority.ToString(),
                Type = target.Type.ToString(),
                Image = ImageModel.FromEntity(target.Image),
            };

            if (target.Type == TargetType.Adjustment)
            {
                if (adjustmentTarget != null)
                {
                    result.AdjustmentTargetId = adjustmentTarget.Id;
                    result.TargetValue = adjustmentTarget.TargetValue;
                    result.Step = adjustmentTarget.Step;
                    result.StepDirection = adjustmentTarget.StepDirection.ToString();
                    result.RecommendedText = adjustmentTarget.RecommendedText;
                    result.TargetText = adjustmentTarget.TargetText;
                    result.RemainText = adjustmentTarget.RemainText;
                }
                else return null;
            }

            return result; 
        }

        public Target ToTargetEntity()
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

        public AdjustmentTarget ToAdjustmentTargetEntity()
        {
            return ToTargetEntity().Type == TargetType.Adjustment ?
            new AdjustmentTarget()
            {
                Id = this.AdjustmentTargetId,
                TargetId = this.Id,
                TargetValue = this.TargetValue,
                Step = this.Step,
                StepDirection = (AdjustmentTargetStepDirection)Enum.Parse(typeof(AdjustmentTargetStepDirection), this.StepDirection),
                RecommendedText = this.RecommendedText,
                TargetText = this.TargetText,
                RemainText = this.RemainText,
            } : null;
        }
    }
}
