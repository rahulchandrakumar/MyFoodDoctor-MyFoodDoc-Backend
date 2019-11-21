using System;
using System.Collections.Generic;

namespace MyFoodDoc.CMS.Application.Models
{
    public class PatientModel: BaseModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Insurance { get; set; }
        public SexEnum Sex { get; set; }
        public int Height { get; set; }
        public DateTime Birth { get; set; }
        public IList<HistoryModel<decimal>> Weight { get; set; }
        public IList<HistoryModel<int>> BloodSugar { get; set; }
        public IList<HistoryModel<decimal>> AbdominalGirth { get; set; }
        public IList<string> Motivation { get; set; }
    }
}
