using System.Collections.Generic;

namespace MyFoodDoc.CMS.Application.Persistence.Base
{
    public class PaginatedItems<T>
    {
        public IList<T> Items { get; set; }

        public long TotalCount { get; set; }
    }
}
