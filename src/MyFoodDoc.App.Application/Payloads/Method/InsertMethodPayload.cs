using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.Method
{
    public class InsertMethodPayload
    {
        public IEnumerable<MethodPayload> Methods { get; set; }
    }
}
