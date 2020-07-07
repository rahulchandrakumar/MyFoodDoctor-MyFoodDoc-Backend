using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Abstractions;
using MyFoodDoc.Application.Entities.Methods;

namespace MyFoodDoc.Application.Entities
{
    public class UserMethod : AbstractAuditableEntity
    {
        public string UserId { get; set; }

        public int MethodId { get; set; }

        public bool? Answer { get; set; }

        public int? IntegerValue { get; set; }

        public decimal? DecimalValue { get; set; }

        public int? MethodMultipleChoiceId { get; set; }

        public User User { get; set; }

        public Method Method { get; set; }
    }
}
