using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence.Base
{
    public interface IServiceBaseWrite<T>
    {
        public Task<T> AddItem(T item, CancellationToken cancellationToken = default);
        public Task<T> UpdateItem(T item, CancellationToken cancellationToken = default);
        public Task<bool> DeleteItem(object id, CancellationToken cancellationToken = default);
    }
}
