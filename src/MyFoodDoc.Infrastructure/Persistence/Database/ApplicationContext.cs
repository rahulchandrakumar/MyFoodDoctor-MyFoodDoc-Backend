using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Aok;
using MyFoodDoc.Application.Entities.Courses;
using MyFoodDoc.Application.Entities.Diary;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Entities.Psychogramm;
using MyFoodDoc.Application.Entities.Subscriptions;
using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.Entities.TrackedValues;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.Infrastructure.Persistence.Database
{
    public class ApplicationContext : IdentityDbContext<User>, IApplicationContext
    {
        public bool WithSeeding { get; set; } = false;

        #region Coupons
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        #endregion

        #region Diary
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealIngredient> MealIngredients { get; set; }
        public DbSet<Liquid> Liquids { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<FavouriteIngredient> FavouriteIngredients { get; set; }
        public DbSet<MealFavourite> MealFavourites { get; set; }
        #endregion

        #region System
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Motivation> Motivations { get; set; }
        public DbSet<Indication> Indications { get; set; }
        public DbSet<Diet> Diets { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<OptimizationArea> OptimizationAreas { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<AdjustmentTarget> AdjustmentTargets { get; set; }
        public DbSet<MotivationTarget> MotivationTargets { get; set; }
        public DbSet<IndicationTarget> IndicationTargets { get; set; }
        public DbSet<DietTarget> DietTargets { get; set; }
        public DbSet<UserTarget> UserTargets { get; set; }
        public DbSet<Method> Methods { get; set; }
        public DbSet<MethodMultipleChoice> MethodMultipleChoice { get; set; }
        public DbSet<MethodText> MethodTexts { get; set; }
        public DbSet<UserMethod> UserMethods { get; set; }
        public DbSet<UserMethodShowHistoryItem> UserMethodShowHistory { get; set; }
        public DbSet<MotivationMethod> MotivationMethods { get; set; }
        public DbSet<IndicationMethod> IndicationMethods { get; set; }
        public DbSet<DietMethod> DietMethods { get; set; }
        public DbSet<TargetMethod> TargetMethods { get; set; }
        public DbSet<UserTimer> UserTimer { get; set; }

        #endregion

        #region Lexicon
        public DbSet<LexiconEntry> LexiconEntries { get; set; }

        public DbSet<LexiconCategory> LexiconCategories { get; set; }
        #endregion

        #region Cms
        public DbSet<CmsUser> CmsUsers { get; set; }
        public DbSet<WebPage> WebPages { get; set; }
        #endregion

        #region Users
        public DbSet<UserMotivation> UserMotivations { get; set; }
        public DbSet<UserIndication> UserIndications { get; set; }
        public DbSet<UserDiet> UserDiets { get; set; }
        public DbSet<UserWeight> UserWeights { get; set; }
        public DbSet<UserAbdominalGirth> UserAbdominalGirths { get; set; }
        public DbSet<AppStoreSubscription> AppStoreSubscriptions { get; set; }
        public DbSet<GooglePlayStoreSubscription> GooglePlayStoreSubscriptions { get; set; }
        #endregion

        #region Courses

        public DbSet<Course> Courses { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Subchapter> Subchapters { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        #endregion

        #region Psychogramm

        public DbSet<Scale> Scales { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<UserChoice> UserChoices { get; set; }

        #endregion

        #region AOK
        public DbSet<AokUser> AokUsers { get; set; }
        #endregion

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //this.Database.EnsureCreated();
        }

        private void BeforeSaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            BeforeSaveChanges();

            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task WithTransaction(Action<IDbContextTransaction> action)
        {
            using (var transaction = await Database.BeginTransactionAsync())
            {
                action.Invoke(transaction);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            //builder.Entity<ReportMethod>().ToQuery(() => ReportValueMethods.Cast<ReportMethod>().AsQueryable().Union(ReportChoiceMethods));

            builder.Entity<IdentityRole<string>>(entity => entity.ToTable("Role", "User"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRole", "User"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaim", "User"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogin", "User"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaim", "User"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserToken", "User"));

            if (WithSeeding)
            {

                builder.Entity<Insurance>().HasData(
                    new Insurance
                    {
                        Id = -1,
                        Name = "Other"
                    },
                    new Insurance
                    {
                        Id = 1,
                        Name = "Aok"
                    },
                    new Insurance
                    {
                        Id = 2,
                        Name = "Barmer"
                    },
                    new Insurance
                    {
                        Id = 3,
                        Name = "DAK"
                    },
                    new Insurance
                    {
                        Id = 4,
                        Name = "hkk"
                    },
                    new Insurance
                    {
                        Id = 5,
                        Name = "Techniker"
                    }
                );

                //,,,diabetes_type_1,
                builder.Entity<Indication>().HasData(
                    new Indication
                    {
                        Id = 1,
                        Key = "hypertension",
                        Name = "Hypertonie"
                    },
                    new Indication
                    {
                        Id = 2,
                        Key = "adipositas",
                        Name = "Adipositas"
                    },
                    new Indication
                    {
                        Id = 3,
                        Key = "diabetes_type_1",
                        Name = "Diabetes Typ 1"
                    },
                    new Indication
                    {
                        Id = 4,
                        Key = "diabetes_type_2",
                        Name = "Diabetes Typ 2"
                    },
                    new Indication
                    {
                        Id = 5,
                        Key = "eating_disorder",
                        Name = "Essstörungen oder psychiatrische Grunderkrankung"
                    }
                );

                builder.Entity<Motivation>().HasData(
                    new Motivation
                    {
                        Id = 1,
                        Key = "anti_aging",
                        Name = "Anti Aging"
                    },
                    new Motivation
                    {
                        Id = 2,
                        Key = "healthier_lifestyle",
                        Name = "Gesünder leben"
                    },
                    new Motivation
                    {
                        Id = 3,
                        Key = "reduce_weight",
                        Name = "Abnehmen"
                    },
                    new Motivation
                    {
                        Id = 4,
                        Key = "feel_better",
                        Name = "Besser fühlen"
                    }
                );

                builder.Entity<Diet>().HasData(
                    new Diet
                    {
                        Id = 1,
                        Key = "vegetarian",
                        Name = "Vegetarisch"
                    },
                    new Diet
                    {
                        Id = 2,
                        Key = "mixed_diet",
                        Name = "Mischkost"
                    },
                    new Diet
                    {
                        Id = 3,
                        Key = "vegan",
                        Name = "Vegan"
                    },
                    new Diet
                    {
                        Id = 4,
                        Key = "less_carbohydrates",
                        Name = "Wenig Kohlenhydrate"
                    },
                    new Diet
                    {
                        Id = 5,
                        Key = "lactose_low",
                        Name = "Laktosearm"
                    }
                );

                builder.Entity<OptimizationArea>().HasData(
                    new OptimizationArea
                    {
                        Id = 1,
                        Key = "vegetables",
                        Name = "Gemüse",
                        Text = "Gemüse und Salat haben einen hohen Wasseranteil, wodurch sie sehr kalorienarm sind. Die enthaltenen Ballaststoffe quellen im Magen-Darm-Trakt auf, wodurch du lange satt bleibst.\n" +
                                "Die Ballaststoffe aus Gemüse und Salat sorgen für eine gute Verdauung, wodurch dein Darm gesund bleibt.\n" +
                                "Gemüse und Salat sind reich an Vitaminen und sekundären Pflanzenstoffen. Sie sorgen für ein starkes Immunsystem.",
                        LineGraphUpperLimit = null,
                        LineGraphLowerLimit = 400,
                        LineGraphOptimal = 500
                    },
                    new OptimizationArea
                    {
                        Id = 2,
                        Key = "sugar",
                        Name = "Zucker",
                        Text = "Zuckerreiche Lebensmittel liefern schnell, aber nur kurzfristige Energie. Es kommt zu einem sehr schnellen Anstieg des Blutzuckers, was zu einer sehr schnellen und starken Insulinausschüttung aus der Bauchspeicheldrüse führt.\n" +
                                "Dieser Mechanismus fördert Heißhungerattacken und Übergewicht.",
                        LineGraphUpperLimit = 40,
                        LineGraphLowerLimit = null,
                        LineGraphOptimal = 30
                    },
                    new OptimizationArea
                    {
                        Id = 3,
                        Key = "protein",
                        Name = "Proteine",
                        Text = "Eiweiß ist ein lebensnotweniger Nährstoff. Es sorgt für den Erhalt und den Aufbau unserer Muskulatur und unterstützt unser Immunsystem.\n" +
                                "Zusätzlich sorgt eine eiweißreiche Mahlzeit für weniger Blutzuckerschwankungen und eine lang anhaltende Sättigung.",
                        LineGraphUpperLimit = 1.5m,
                        LineGraphLowerLimit = 0.8m,
                        LineGraphOptimal = 1
                    },
                    new OptimizationArea
                    {
                        Id = 4,
                        Key = "snacking",
                        Name = "Snacking",
                        Text = "Snacking",
                        LineGraphUpperLimit = null,
                        LineGraphLowerLimit = null,
                        LineGraphOptimal = null
                    });
            }
        }
    }
}
