using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Abstractions;

namespace MyFoodDoc.Application.Entities.Methods
{
    public class MethodMultipleChoice : AbstractAuditableEntity
    {
        public int MethodId { get; set; }

        public string Title { get; set; }

        public bool IsCorrect { get; set; }

        public Method Method { get; set; }
    }
}
