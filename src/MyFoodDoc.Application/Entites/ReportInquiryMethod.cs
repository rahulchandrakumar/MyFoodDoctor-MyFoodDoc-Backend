using MyFoodDoc.Application.Entites.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public class ReportInquiryMethod : ReportValueMethod
    {
        public override ValueMethodType Type { get; } = ValueMethodType.Inquiry;

        public new bool? Value => IsConfirmed;

        public new InquiryMethod Method { get; protected set; }

        public bool? IsConfirmed { get; set; }
    }
}
