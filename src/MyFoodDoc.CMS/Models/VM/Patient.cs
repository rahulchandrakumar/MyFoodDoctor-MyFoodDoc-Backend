using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Patient : VMBase.BaseModel<string>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? InsuranceId { get; set; }
        public byte? Gender { get; set; }
        public int Height { get; set; }
        public DateTime? Birth { get; set; }
        public IList<HistoryBaseModel<decimal>> Weight { get; set; }
        //public IList<HistoryBaseModel<int>> BloodSugar { get; set; }
        public IList<HistoryBaseModel<int>> AbdominalGirth { get; set; }
        public IList<string> Motivation { get; set; }

        public static Patient FromModel(PatientModel model)
        {
            return model == null ? null : new Patient()
            {
                Id = model.Id,
                FullName = model.FullName,
                Birth = model.Birth,
                Email = model.Email,
                InsuranceId = model.InsuranceId,
                Height = model.Height ?? 0,
                Gender = (byte?)model.Gender,
                AbdominalGirth = model.AbdominalGirth?.Select(HistoryBaseModel<int>.FromModel).ToList(),
                //BloodSugar = model.BloodSugar?.Select(HistoryBaseModel<int>.FromModel).ToList(),
                Weight = model.Weight?.Select(HistoryBaseModel<decimal>.FromModel).ToList(),
                Motivation = model.Motivation?.ToList()
            };
        }
    }
}
