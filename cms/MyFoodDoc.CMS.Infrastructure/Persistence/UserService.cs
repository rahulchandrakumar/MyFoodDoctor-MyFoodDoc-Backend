using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class UserService : IUserService
    {
        private readonly IApplicationContext _context;
        public UserService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<UserModel> AddItem(UserModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();

            await _context.CmsUsers.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return UserModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.CmsUsers.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (entity == null)
                return false;
            
            _context.CmsUsers.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<UserModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            return UserModel.FromEntity(await _context.CmsUsers.FirstOrDefaultAsync(u => u.Id == id, cancellationToken));
        }

        public async Task<IList<UserModel>> GetItems(CancellationToken cancellationToken = default)
        {
            return (await _context.CmsUsers.ToListAsync(cancellationToken)).Select(UserModel.FromEntity).ToList();
        }

        public async Task<UserModel> UpdateItem(UserModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.CmsUsers.FirstOrDefaultAsync(u => u.Id == item.Id, cancellationToken);
            var passHash = entity.PasswordHash;

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            if (string.IsNullOrEmpty(item.PasswordHash))
                entity.PasswordHash = passHash;

            await _context.SaveChangesAsync(cancellationToken);

            return UserModel.FromEntity(entity);
        }
    }
}
