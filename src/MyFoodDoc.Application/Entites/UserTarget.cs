using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public class UserTarget : AbstractAuditableEntity
    {
        public string UserId { get; set; }

        public int TargetId { get; set; }

        public string TargetAnswerCode { get; set; }

        public User User { get; set; }

        public Target Target { get; set; }
    }
}
