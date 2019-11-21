using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Patient : ColabDataTableBaseModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Insurance { get; set; }
        public byte Sex { get; set; }
        public int Height { get; set; }
        public DateTime Birth { get; set; }
        public IList<HistoryBaseModel<decimal>> Weight { get; set; }
        public IList<HistoryBaseModel<int>> BloodSugar { get; set; }
        public IList<HistoryBaseModel<decimal>> AbdominalGirth { get; set; }
        public IList<string> Motivation { get; set; }

        public static Patient FromModel(PatientModel model)
        {
            return new Patient()
            {
                Id = model.Id,
                FullName = model.FullName,
                Birth = model.Birth,
                Email = model.Email,
                Insurance = model.Insurance,
                Height = model.Height,
                Sex = (byte)model.Sex,
                AbdominalGirth = model.AbdominalGirth?.Select(HistoryBaseModel<decimal>.FromModel).ToList(),
                BloodSugar = model.BloodSugar?.Select(HistoryBaseModel<int>.FromModel).ToList(),
                Weight = model.Weight?.Select(HistoryBaseModel<decimal>.FromModel).ToList(),
                Motivation = model.Motivation?.ToList()
            };
        }

        public PatientModel ToModel()
        {
            return new PatientModel()
            {
                Id = Id,
                FullName = FullName,
                Birth = Birth,
                Email = Email,
                Insurance = Insurance,
                Height = Height,
                Sex = (SexEnum)Sex,
                AbdominalGirth = AbdominalGirth?.Select(x => x.ToModel()).ToList(),
                BloodSugar = BloodSugar?.Select(x => x.ToModel()).ToList(),
                Weight = Weight?.Select(x => x.ToModel()).ToList()
            };
        }
    }
}
