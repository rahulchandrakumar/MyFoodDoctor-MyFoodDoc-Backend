using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Method
{
    public class InsertMethodPayload
    {
        public IEnumerable<MethodPayload> Methods { get; set; }
    }
}
