using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Payloads.Aok;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Aok;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class AokService : IAokService
    {
        private readonly IApplicationContext _context;

        public AokService(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string userId)
        {
            return await _context.AokUsers.AnyAsync(x => x.UserId == userId);
        }

        public async Task InsertUserAsync(string userId, AokUserPayload aokUserPayload, CancellationToken cancellationToken)
        {
            await _context.AokUsers.AddAsync(new AokUser
            {
                UserId = userId,
                Token = aokUserPayload.Token
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
