using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.Target
{
    public class InsertTargetPayload
    {
        public IEnumerable<TargetPayload> Targets { get; set; }
    }
}
