using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Application.Persistence.Base;
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

        public IQueryable<CmsUser> GetBaseQuery(string search)
        {
            IQueryable<CmsUser> baseQuery = _context.CmsUsers;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Username, searchstring) || EF.Functions.Like(f.Displayname, searchstring));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<UserModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<UserModel>()
            {
                Items = entities.Skip(skip).Take(take).Select(UserModel.FromEntity).ToList(),
                TotalCount = entities.Count
            };
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

        public async Task<UserModel> GetByUsername(string userName, CancellationToken cancellationToken = default)
        {
            return UserModel.FromEntity(await _context.CmsUsers.FirstOrDefaultAsync(u => u.Username == userName, cancellationToken));
        }
    }
}
