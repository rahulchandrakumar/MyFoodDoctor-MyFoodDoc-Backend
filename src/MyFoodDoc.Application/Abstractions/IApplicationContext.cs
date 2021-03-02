using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Courses;
using MyFoodDoc.Application.Entities.Diary;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Entities.Psychogramm;
using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.Entities.TrackedValues;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IApplicationContext
    {
        DatabaseFacade Database { get; }

        #region Coupons
        DbSet<Promotion> Promotions { get; set; }
        DbSet<Coupon> Coupons { get; set; }
        #endregion

        #region Diary
        DbSet<Meal> Meals { get; set; }
        DbSet<MealIngredient> MealIngredients { get; set; }
        DbSet<Liquid> Liquids { get; set; }
        DbSet<Exercise> Exercises { get; set; }
        DbSet<Favourite> Favourites { get; set; }
        DbSet<FavouriteIngredient> FavouriteIngredients { get; set; }
        DbSet<MealFavourite> MealFavourites { get; set; }
        #endregion

        #region System
        DbSet<Insurance> Insurances { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Motivation> Motivations { get; set; }
        DbSet<Indication> Indications { get; set; }
        DbSet<Diet> Diets { get; set; }
        DbSet<Ingredient> Ingredients { get; set; }
        DbSet<OptimizationArea> OptimizationAreas { get; set; }
        DbSet<Target> Targets { get; set; }
        DbSet<AdjustmentTarget> AdjustmentTargets { get; set; }
        DbSet<MotivationTarget> MotivationTargets { get; set; }
        DbSet<IndicationTarget> IndicationTargets { get; set; }
        DbSet<DietTarget> DietTargets { get; set; }
        DbSet<UserTarget> UserTargets { get; set; }
        DbSet<Method> Methods { get; set; }
        DbSet<MethodMultipleChoice> MethodMultipleChoice { get; set; }
        DbSet<MethodText> MethodTexts { get; set; }
        DbSet<UserMethod> UserMethods { get; set; }
        DbSet<UserMethodShowHistoryItem> UserMethodShowHistory { get; set; }
        DbSet<MotivationMethod> MotivationMethods { get; set; }
        DbSet<IndicationMethod> IndicationMethods { get; set; }
        DbSet<DietMethod> DietMethods { get; set; }
        DbSet<TargetMethod> TargetMethods { get; set; }
        DbSet<UserTimer> UserTimer { get; set; }

        #endregion

        #region Lexicon
        DbSet<LexiconEntry> LexiconEntries { get; set; }

        DbSet<LexiconCategory> LexiconCategories { get; set; }
        #endregion

        #region Cms
        DbSet<CmsUser> CmsUsers { get; set; }
        DbSet<WebPage> WebPages { get; set; }
        #endregion

        #region Users
        DbSet<User> Users { get; set; }
        DbSet<UserMotivation> UserMotivations { get; set; }
        DbSet<UserIndication> UserIndications { get; set; }
        DbSet<UserDiet> UserDiets { get; set; }
        DbSet<UserWeight> UserWeights { get; set; }
        DbSet<UserAbdominalGirth> UserAbdominalGirths { get; set; }
        #endregion

        #region Courses

        DbSet<Course> Courses { get; set; }
        DbSet<Chapter> Chapters { get; set; }
        DbSet<Subchapter> Subchapters { get; set; }
        DbSet<UserAnswer> UserAnswers { get; set; }

        #endregion

        #region Psychogramm

        DbSet<Scale> Scales { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Choice> Choices { get; set; }
        DbSet<UserChoice> UserChoices { get; set; }

        #endregion

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task WithTransaction(Action<IDbContextTransaction> action);

        EntityEntry Entry([NotNullAttribute] object entity);

        EntityEntry<TEntity> Entry<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
    }
}
