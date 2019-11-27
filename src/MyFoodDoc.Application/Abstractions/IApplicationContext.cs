using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.TrackedValus;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IApplicationContext
    {
        DatabaseFacade Database { get; }
        DbSet<Coupon> Coupons { get; set; }
        DbSet<Meal> Meals { get; set; }
        DbSet<MealIngredient> MealIngredients { get; set; }
        DbSet<Liquid> Liquids { get; set; }
        DbSet<Exercise> Exercises { get; set; }
        DbSet<Diet> Diets { get; set; }
        DbSet<Motivation> Motivations { get; set; }
        DbSet<Indication> Indications { get; set; }
        DbSet<Ingredient> Ingredients { get; set; }
        DbSet<Insurance> Insurances { get; set; }
        DbSet<LexiconEntry> LexiconEntries { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<CmsUser> CmsUsers { get; set; }
        DbSet<UserMotivation> UserMotivations { get; set; }
        DbSet<UserIndication> UserIndications { get; set; }
        DbSet<UserDiet> UserDiets { get; set; }
        DbSet<WebPage> WebPages { get; set; }
        //DbSet<UserWeight> UserWeights { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task WithTransaction(Action<IDbContextTransaction> action);

        EntityEntry Entry([NotNullAttribute] object entity);

        EntityEntry<TEntity> Entry<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;

    }
}
