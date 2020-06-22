using MyFoodDoc.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.CMS.Application.Models
{
    public class PatientModel: BaseModel<string>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? InsuranceId { get; set; }
        public GenderEnum? Gender { get; set; }
        public decimal? Height { get; set; }
        public DateTime? Birth { get; set; }
        public IList<HistoryModel<decimal>> Weight { get; set; }
        public IList<HistoryModel<decimal>> AbdominalGirth { get; set; }
        public IList<string> Motivations { get; set; }
        public IList<string> Indications { get; set; }

        public static PatientModel FromEntity(User entity)
        {
            return entity == null ? null : new PatientModel()
            {
                Id = entity.Id,
                Birth = entity.Birthday,
                Email = entity.Email,
                FullName = entity.UserName,
                InsuranceId = entity.InsuranceId,
                Gender = entity.Gender == null ? null : (GenderEnum?)Enum.Parse(typeof(GenderEnum), entity.Gender?.ToString()),
                Height = entity.Height,
                AbdominalGirth = entity.AbdominalGirthHistory?.Select(HistoryModel<decimal>.FromEntity).ToList(),
                Weight = entity.WeightHistory?.Select(HistoryModel<decimal>.FromEntity).ToList(),
                Motivations = entity.Motivations?.Select(x => x.Motivation.Name).ToList(),
                Indications = entity.Indications?.Select(x => x.Indication.Name).ToList()
            };
        }
    }
}
