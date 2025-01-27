﻿using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Target : VMBase.BaseModel<int>
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
        public bool? ZppSubscription { get; set; }

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

        public Image Image { get; set; }

        public static Target FromModel(TargetModel model)
        {
            return model == null ? null : new Target()
            {
                Id = model.Id,
                OptimizationAreaId = model.OptimizationAreaId,
                Title = model.Title,
                Text = model.Text,
                IsMandatory = model.IsMandatory,
                TriggerOperator = model.TriggerOperator,
                TriggerValue = model.TriggerValue,
                Threshold = model.Threshold,
                Priority = model.Priority,
                Type = model.Type,
                AdjustmentTargetId = model.AdjustmentTargetId,
                TargetValue = model.TargetValue,
                Step = model.Step,
                StepDirection = model.StepDirection,
                RecommendedText = model.RecommendedText,
                TargetText = model.TargetText,
                RemainText = model.RemainText,
                ZppSubscription = model.ZppSubscription,
                Image = Image.FromModel(model.Image),
                Diets = model.Diets?.ToList(),
                Indications = model.Indications?.ToList(),
                Motivations = model.Motivations?.ToList()
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
                IsMandatory = this.IsMandatory,
                TriggerOperator = this.TriggerOperator,
                TriggerValue = this.TriggerValue,
                Threshold = this.Threshold,
                Priority = this.Priority,
                Type = this.Type,
                AdjustmentTargetId = this.AdjustmentTargetId,
                TargetValue = this.TargetValue,
                Step = this.Step,
                StepDirection = this.StepDirection,
                RecommendedText = this.RecommendedText,
                TargetText = this.TargetText,
                RemainText = this.RemainText,
                ZppSubscription = this.ZppSubscription,
                Image = this.Image.ToModel(),
                Diets = this.Diets?.ToList(),
                Indications = this.Indications?.ToList(),
                Motivations = this.Motivations?.ToList()
            };
        }
    }
}
