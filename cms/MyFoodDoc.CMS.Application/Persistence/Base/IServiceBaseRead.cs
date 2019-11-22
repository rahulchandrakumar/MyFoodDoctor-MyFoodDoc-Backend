using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence.Base
{
    public interface IServiceBaseRead<T>
    {
        public Task<IList<T>> GetItems(CancellationToken cancellationToken = default);
        public Task<T> GetItem(object id, CancellationToken cancellationToken = default);
    }
}
