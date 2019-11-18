using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IApplicationContext
    {
        DbSet<Coupon> Coupons { get; set; }

        DbSet<Diet> Diets { get; set; }

        DbSet<Ingredient> Ingredients { get; set; }

        DbSet<Insurance> Insurances { get; set; }

        DbSet<LexiconEntry> LexiconEntries { get; set; }

        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
