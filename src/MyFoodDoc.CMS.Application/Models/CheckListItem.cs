using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.CMS.Application.Models
{
    public class CheckListItem<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}
