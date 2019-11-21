using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence.Base
{
    public interface IServiceBaseRead<T>
    {
        public Task<IList<T>> GetItems();
        public Task<T> GetItem(object id);
    }
}
