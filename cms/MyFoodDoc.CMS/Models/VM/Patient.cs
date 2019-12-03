using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Patient : ColabDataTableBaseModel<string>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? InsuranceId { get; set; }
        public byte? Sex { get; set; }
        public int Height { get; set; }
        public DateTime? Birth { get; set; }
        public IList<HistoryBaseModel<int>> Weight { get; set; }
        public IList<HistoryBaseModel<int>> BloodSugar { get; set; }
        public IList<HistoryBaseModel<int>> AbdominalGirth { get; set; }
        public IList<string> Motivation { get; set; }

        public static Patient FromModel(PatientModel model)
        {
            return new Patient()
            {
                Id = model.Id,
                FullName = model.FullName,
                Birth = model.Birth,
                Email = model.Email,
                InsuranceId = model.InsuranceId,
                Height = model.Height ?? 0,
                Sex = (byte?)model.Sex,
                AbdominalGirth = model.AbdominalGirth?.Select(HistoryBaseModel<int>.FromModel).ToList(),
                BloodSugar = model.BloodSugar?.Select(HistoryBaseModel<int>.FromModel).ToList(),
                Weight = model.Weight?.Select(HistoryBaseModel<int>.FromModel).ToList(),
                Motivation = model.Motivation?.ToList()
            };
        }
    }
}
