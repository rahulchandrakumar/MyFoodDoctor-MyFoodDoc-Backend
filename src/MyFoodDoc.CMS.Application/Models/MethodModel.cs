using System;
using System.Collections.Generic;
using System.Linq;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.CMS.Application.Models
{
    public class MethodModel : BaseModel<int>
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int? Frequency { get; set; }
        public string FrequencyPeriod { get; set; }
        public ImageModel Image { get; set; }
        public IList<int> Targets { get; set; }
        public IList<int> Diets { get; set; }
        public IList<int> Indications { get; set; }
        public IList<int> Motivations { get; set; }

        public static MethodModel FromEntity(Method entity)
        {
            return entity == null ? null : new MethodModel()
            {
                Id = entity.Id,
                Type = entity.Type.ToString(),
                Title = entity.Title,
                Text = entity.Text,
                Frequency = entity.Frequency,
                FrequencyPeriod = entity.FrequencyPeriod?.ToString(),
                Image = entity.Image == null ? null : ImageModel.FromEntity(entity.Image),
                Targets = entity.Targets?.Select(x => x.TargetId).ToList(),
                Diets = entity.Diets?.Select(x => x.DietId).ToList(),
                Indications = entity.Indications?.Select(x => x.IndicationId).ToList(),
                Motivations = entity.Motivations?.Select(x => x.MotivationId).ToList()
            };
        }

        public Method ToEntity()
        {
            return new Method()
            {
                Id = this.Id,
                Type = Enum.Parse<MethodType>(this.Type),
                Title = this.Title,
                Text = this.Text,
                Frequency = this.Frequency,
                FrequencyPeriod = string.IsNullOrEmpty(this.FrequencyPeriod) ? (MethodFrequencyPeriod?)null : Enum.Parse<MethodFrequencyPeriod>(this.FrequencyPeriod),
                ImageId = this.Image == null || string.IsNullOrEmpty(this.Image.Url) ? (int?)null : this.Image.Id,
            };
        }

        public IList<TargetMethod> ToTargetMethodEntities()
        {
            return this.Targets?.Select(x => new TargetMethod { TargetId = x, MethodId = this.Id }).ToList();
        }

        public IList<DietMethod> ToDietMethodEntities()
        {
            return this.Diets?.Select(x => new DietMethod { DietId = x, MethodId = this.Id }).ToList();
        }

        public IList<IndicationMethod> ToIndicationMethodEntities()
        {
            return this.Indications?.Select(x => new IndicationMethod { IndicationId = x, MethodId = this.Id }).ToList();
        }

        public IList<MotivationMethod> ToMotivationMethodEntities()
        {
            return this.Motivations?.Select(x => new MotivationMethod { MotivationId = x, MethodId = this.Id }).ToList();
        }
    }
}
