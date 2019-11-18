using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IEnumEntity : IAuditEntity<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }
    }
}
