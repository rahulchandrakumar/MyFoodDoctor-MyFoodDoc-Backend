using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence.Base
{
    public interface IServiceBasePaginatedRead<T1, T2>
    {
        public Task<IList<T1>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default);
        public Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default);
        public Task<T1> GetItem(T2 id, CancellationToken cancellationToken = default);
    }
}
