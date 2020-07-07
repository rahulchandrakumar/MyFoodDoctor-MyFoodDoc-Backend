using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Target
{
    public class InsertTargetPayload
    {
        public IEnumerable<TargetPayload> Targets { get; set; }
    }
}
