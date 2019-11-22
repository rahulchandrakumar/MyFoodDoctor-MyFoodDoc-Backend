using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence.Base
{
    public interface IServiceBaseWrite<T1, T2>
    {
        public Task<T1> AddItem(T1 item, CancellationToken cancellationToken = default);
        public Task<T1> UpdateItem(T1 item, CancellationToken cancellationToken = default);
        public Task<bool> DeleteItem(T2 id, CancellationToken cancellationToken = default);
    }
}
