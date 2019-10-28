using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Motivation : AbstractAuditEntity<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }
    }
}
