using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IServiceBase<T>
    {
        public Task<IList<T>> GetItems();
        public Task<T> GetItem(int id);
        public Task<T> AddItem(T item);
        public Task<T> UpdateItem(T item);
        public Task<bool> DeleteItem(int id);
    }
}
