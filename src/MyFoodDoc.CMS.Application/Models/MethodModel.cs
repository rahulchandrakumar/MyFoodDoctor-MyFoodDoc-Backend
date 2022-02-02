using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.CMS.Application.Models
{
    public class MethodModel : BaseModel<int>
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int? Frequency { get; set; }
        public string FrequencyPeriod { get; set; }
        public int? ParentId { get; set; }
        public int? TimeIntervalDay { get; set; }
        public int? TimeIntervalNight { get; set; }
        public bool IsActive { get; set; }
        public ImageModel Image { get; set; }
        public IList<int> Targets { get; set; }
        public IList<int> Diets { get; set; }
        public IList<int> ContraindicatedDiets { get; set; }
        public IList<int> Indications { get; set; }
        public IList<int> Contraindications { get; set; }
        public IList<int> Motivations { get; set; }
        public IList<int> ContraindicatedMotivations { get; set; }

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
                ParentId = entity.ParentId,
                TimeIntervalDay = entity.TimeIntervalDay,
                TimeIntervalNight = entity.TimeIntervalNight,
                IsActive = entity.IsActive,
                Image = entity.Image == null ? null : ImageModel.FromEntity(entity.Image),
                Targets = entity.Targets?.Select(x => x.TargetId).ToList(),
                Diets = entity.Diets?.Where(x => !x.IsContraindication).Select(x => x.DietId).ToList(),
                ContraindicatedDiets = entity.Diets?.Where(x => x.IsContraindication).Select(x => x.DietId).ToList(),
                Indications = entity.Indications?.Where(x => !x.IsContraindication).Select(x => x.IndicationId).ToList(),
                Contraindications = entity.Indications?.Where(x => x.IsContraindication).Select(x => x.IndicationId).ToList(),
                Motivations = entity.Motivations?.Where(x => !x.IsContraindication).Select(x => x.MotivationId).ToList(),
                ContraindicatedMotivations = entity.Motivations?.Where(x => x.IsContraindication).Select(x => x.MotivationId).ToList()
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
                ParentId = this.ParentId,
                TimeIntervalDay = this.TimeIntervalDay,
                TimeIntervalNight = this.TimeIntervalNight,
                IsActive = this.IsActive,
                ImageId = this.Image == null || string.IsNullOrEmpty(this.Image.Url) ? (int?)null : this.Image.Id,
            };
        }

        public IList<TargetMethod> ToTargetMethodEntities()
        {
            return this.Targets?.Select(x => new TargetMethod { TargetId = x, MethodId = this.Id }).ToList();
        }

        public IList<DietMethod> ToDietMethodEntities()
        {
            var diets = this.Diets != null ? this.Diets.Select(x => new DietMethod
            { DietId = x, MethodId = this.Id, IsContraindication = false }).ToList() : new List<DietMethod>();

            var contraindicatedDiets = this.ContraindicatedDiets != null ? this.ContraindicatedDiets.Select(x => new DietMethod
            { DietId = x, MethodId = this.Id, IsContraindication = true }).ToList() : new List<DietMethod>();

            return diets.Union(contraindicatedDiets).ToList();
        }

        public IList<IndicationMethod> ToIndicationMethodEntities()
        {
            var indications = this.Indications != null
                ? this.Indications.Select(x => new IndicationMethod
                { IndicationId = x, MethodId = this.Id, IsContraindication = false }).ToList()
                : new List<IndicationMethod>();

            var contraindications = this.Contraindications != null
                ? this.Contraindications.Select(x => new IndicationMethod
                { IndicationId = x, MethodId = this.Id, IsContraindication = true }).ToList()
                : new List<IndicationMethod>();

            return indications.Union(contraindications).ToList();
        }

        public IList<MotivationMethod> ToMotivationMethodEntities()
        {
            var motivations = this.Motivations != null
                ? this.Motivations.Select(x => new MotivationMethod
                { MotivationId = x, MethodId = this.Id, IsContraindication = false })
                : new List<MotivationMethod>();

            var contraindicatedMotivations = this.ContraindicatedMotivations != null
                ? this.ContraindicatedMotivations.Select(x => new MotivationMethod
                { MotivationId = x, MethodId = this.Id, IsContraindication = true })
                : new List<MotivationMethod>();

            return motivations.Union(contraindicatedMotivations).ToList();
        }
    }
}
