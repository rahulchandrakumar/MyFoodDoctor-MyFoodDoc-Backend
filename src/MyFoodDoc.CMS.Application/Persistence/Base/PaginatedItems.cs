using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.CMS.Application.Persistence.Base
{
    public class PaginatedItems<T>
    {
        public IList<T> Items { get; set; }

        public long TotalCount { get; set; }
    }
}
