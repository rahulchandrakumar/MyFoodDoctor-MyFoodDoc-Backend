using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Method
{
    public class MethodPayload
    {
        public int Id { get; set; }

        public bool? UserAnswer { get; set; }

        public IEnumerable<MethodMultipleChoicePayload> Choices { get; set; }
    }
}
