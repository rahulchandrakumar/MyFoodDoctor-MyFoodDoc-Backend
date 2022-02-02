using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence.Base
{
    public interface IServiceBasePaginatedRead<T1, T2> : IServiceBaseReadItem<T1, T2>
    {
        public Task<PaginatedItems<T1>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default);
    }

    public interface IServiceBasePaginatedRead<T1, T2, T3> : IServiceBaseReadItem<T1, T2>
    {
        public Task<PaginatedItems<T1>> GetItems(int take, int skip, string search, T3 filter, CancellationToken cancellationToken = default);
    }
}
