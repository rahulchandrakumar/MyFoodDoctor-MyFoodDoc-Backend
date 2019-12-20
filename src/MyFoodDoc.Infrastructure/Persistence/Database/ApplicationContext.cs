using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.TrackedValus;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.Infrastructure.Persistence.Database
{
    public class ApplicationContext : IdentityDbContext<User>, IApplicationContext
    {
        public bool WithSeeding { get; set; } = false;

        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Indication> Indications { get; set; }
        public DbSet<Motivation> Motivations { get; set; }
        public DbSet<Diet> Diets { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealIngredient> MealIngredients { get; set; }
        public DbSet<Liquid> Liquids { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<LexiconEntry> LexiconEntries { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<UserMotivation> UserMotivations { get; set; }
        public DbSet<UserIndication> UserIndications { get; set; }
        public DbSet<UserDiet> UserDiets { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<WebPage> WebPages { get; set; }
        public DbSet<CmsUser> CmsUsers { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<UserWeight> UserWeights { get; set; }
        public DbSet<UserAbdominalGirth> UserAbdominalGirths { get; set; }

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
                        Key = "gout",
                        Name = "Gicht"
                    },

                    new Indication
                    {
                        Id = 4,
                        Key = "diabetes_type_1",
                        Name = "Diabetes Typ 1"
                    },
                    new Indication
                    {
                        Id = 5,
                        Key = "diabetes_type_2",
                        Name = "Diabetes Typ 2"
                    }
                );

                builder.Entity<Motivation>().HasData(
                    new Motivation
                    {
                        Id = 1,
                        Key = "anti_aging",
                        Name = "Anti-Aging"
                    },
                    new Motivation
                    {
                        Id = 2,
                        Key = "healthier_lifestyle",
                        Name = "Healthier lifestyle"
                    },
                    new Motivation
                    {
                        Id = 3,
                        Key = "reduce_weight",
                        Name = "Reduce weight"
                    },
                    new Motivation
                    {
                        Id = 4,
                        Key = "feel_better",
                        Name = "Feel better"
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
                        Key = "vegan",
                        Name = "Vegan"
                    },
                    new Diet
                    {
                        Id = 3,
                        Key = "interval_fasting",
                        Name = "Intervallfasten"
                    },
                    new Diet
                    {
                        Id = 4,
                        Key = "vegetarian_milk",
                        Name = "Vegetarisch mit Milch, Ei, Fisch"
                    },
                    new Diet
                    {
                        Id = 5,
                        Key = "lactose_free",
                        Name = "Laktosefrei"
                    },
                    new Diet
                    {
                        Id = 6,
                        Key = "pescetarian",
                        Name = "Vegetarisch mit Fisch"
                    },
                    new Diet
                    {
                        Id = 7,
                        Key = "low_fructose",
                        Name = "Frustosearm"
                    }
                );                

                var resourceName = "MyFoodDoc.Infrastructure.Persistence.Database.Seed.Ingredients.csv";
                var assembly = Assembly.GetExecutingAssembly();

                foreach (var x in assembly.GetManifestResourceNames())
                {
                    Console.WriteLine("1: " + x);
                }

                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream, Encoding.UTF8);
                using var csv = new CsvReader(reader);

                csv.Configuration.RegisterClassMap<IngredientsMap>();
                csv.Configuration.BadDataFound = null;
                csv.Configuration.Delimiter = ",";

                var ingredients = csv.GetRecords<Ingredient>().Where(x => !x.ExternalKey.EndsWith("00000")).ToArray();
                var id = 1;
                foreach (var ingredient in ingredients)
                {
                    ingredient.Id = id++;
                }
                builder.Entity<Ingredient>().HasData(ingredients);
            }
        }

        class IngredientsMap : ClassMap<Ingredient>
        {
            public IngredientsMap()
            {
                Map(x => x.ExternalKey).Index(0);
                Map(x => x.Name).Index(1);
            }
        }
    }
}
