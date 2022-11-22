using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

using Enums = MyFoodDoc.Application.Enums;

namespace MyFoodDoc.CMS.Application.Models
{
    public class TargetModel : BaseModel<int>
    {
        public int OptimizationAreaId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsMandatory { get; set; }
        public string TriggerOperator { get; set; }
        public decimal TriggerValue { get; set; }
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

        public IList<int> Diets { get; set; }
        public IList<int> Indications { get; set; }
        public IList<int> Motivations { get; set; }

        public ImageModel Image { get; set; }

        public static TargetModel FromEntity(Target target, AdjustmentTarget adjustmentTarget)
        {
            if (target == null)
                return null;

            //Target
            var result = new TargetModel()
            {
                Id = target.Id,
                OptimizationAreaId = target.OptimizationAreaId,
                Title = target.Title,
                Text = target.Text,
                TriggerOperator = target.TriggerOperator != Enums.TriggerOperator.Always ? target.TriggerOperator.ToString() : null,
                IsMandatory = target.TriggerOperator == Enums.TriggerOperator.Always,
                TriggerValue = target.TriggerValue,
                Threshold = target.Threshold,
                Priority = target.Priority.ToString(),
                Type = target.Type.ToString(),
                Image = ImageModel.FromEntity(target.Image),
                Diets = target.Diets?.Select(x => x.DietId).ToList(),
                Indications = target.Indications?.Select(x => x.IndicationId).ToList(),
                Motivations = target.Motivations?.Select(x => x.MotivationId).ToList()
            };

            //AdjustmentTarget
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
                TriggerOperator = this.IsMandatory ? Enums.TriggerOperator.Always : Enum.Parse<TriggerOperator>(this.TriggerOperator),
                TriggerValue = this.TriggerValue,
                Threshold = this.Threshold,
                Priority = String.IsNullOrEmpty(this.Priority) ? TargetPriority.High : Enum.Parse<TargetPriority>(this.Priority),
                Type = this.IsMandatory ? Enums.TargetType.Custom : Enum.Parse<TargetType>(this.Type),
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
                StepDirection = Enum.Parse<AdjustmentTargetStepDirection>(this.StepDirection),
                RecommendedText = this.RecommendedText,
                TargetText = this.TargetText,
                RemainText = this.RemainText,
            } : null;
        }

        public IList<DietTarget> ToDietTargetEntities()
        {
            return this.Diets?.Select(x => new DietTarget { DietId = x, TargetId = this.Id }).ToList();
        }

        public IList<IndicationTarget> ToIndicationTargetEntities()
        {
            return this.Indications?.Select(x => new IndicationTarget { IndicationId = x, TargetId = this.Id }).ToList();
        }

        public IList<MotivationTarget> ToMotivationTargetEntities()
        {
            return this.Motivations?.Select(x => new MotivationTarget { MotivationId = x, TargetId = this.Id }).ToList();
        }
    }
}
