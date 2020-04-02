using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Methods
{
    public class InquiryMethod : ValueMethod
    {
        public override ValueMethodType Type => ValueMethodType.Inquiry;
    }
}
