using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.Mock;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class UserService : IUserService
    {
        public async Task<UserModel> AddItem(UserModel item)
        {
            item.Id = UsersMock.Default.Count == 0 ? 0 : (UsersMock.Default.Max(u => u.Id) + 1);
            UsersMock.Default.Add(item);
            return await Task.FromResult(item);
        }

        public async Task<bool> DeleteItem(int id)
        {
            var user = UsersMock.Default.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return await Task.FromResult(false);

            UsersMock.Default.Remove(user);
            return await Task.FromResult(true);
        }

        public async Task<UserModel> GetItem(int id)
        {
            return await Task.FromResult(UsersMock.Default.FirstOrDefault(u => u.Id == id));
        }

        public async Task<IList<UserModel>> GetItems()
        {
            return await Task.FromResult(UsersMock.Default);
        }

        public async Task<UserModel> UpdateItem(UserModel item)
        {
            var user = UsersMock.Default.FirstOrDefault(u => u.Id == item.Id);

            if (user == null)
                return null;

            UsersMock.Default.Remove(user);
            UsersMock.Default.Add(item);
            return await Task.FromResult(item);
        }
    }
}
