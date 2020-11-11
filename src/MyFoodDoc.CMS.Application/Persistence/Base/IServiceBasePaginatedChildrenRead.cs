using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence.Base
{
    public interface IServiceBasePaginatedChildrenRead<T1, T2, T3>
    {
        public Task<PaginatedItems<T1>> GetItems(T2 parentId, int take, int skip, string search, CancellationToken cancellationToken = default);
        public Task<T1> GetItem(T3 id, CancellationToken cancellationToken = default);
    }
}
