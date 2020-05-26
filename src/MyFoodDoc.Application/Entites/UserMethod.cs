using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.Entites.Methods;

namespace MyFoodDoc.Application.Entites
{
    public class UserMethod : AbstractAuditableEntity
    {
        public string UserId { get; set; }

        public int MethodId { get; set; }

        public bool Answer { get; set; }

        public int? MethodMultipleChoiceId { get; set; }

        public User User { get; set; }

        public Method Method { get; set; }
    }
}
