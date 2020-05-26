using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Abstractions;

namespace MyFoodDoc.Application.Entites.Methods
{
    public class MethodMultipleChoice : AbstractAuditableEntity
    {
        public int MethodId { get; set; }

        public string Question { get; set; }

        public bool Answer { get; set; }

        public Method Method { get; set; }
    }
}
