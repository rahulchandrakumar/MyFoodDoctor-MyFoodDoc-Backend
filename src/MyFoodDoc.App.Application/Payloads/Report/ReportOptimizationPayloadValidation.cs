using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Report
{
    public class ReportOptimizationPayloadValidation : AbstractValidator<ReportOptimizationPayload>
    {
        public ReportOptimizationPayloadValidation()
        {

        }
    }
}
