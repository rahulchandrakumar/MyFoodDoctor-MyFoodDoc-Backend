using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.Infrastructure.Persistence.Database
{
    public class ApplicationContext : IdentityDbContext<User>, IApplicationContext
    {
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
        //public DbSet<UserWeight> UserWeights { get; set; }

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

            builder.Entity<Insurance>().HasData(
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

            builder.Entity<Image>().HasData(
                new Image
                {
                    Id = 1,
                    Url = "https://myfooddoctormockcmsimgs.blob.core.windows.net/images/253f35f4-f3ac-425c-93ff-6edfdb62a12f.jpg"
                },
                new Image
                {
                    Id = 2,
                    Url = "https://myfooddoctormockcmsimgs.blob.core.windows.net/images/a5ec0b6f-d3cc-4a52-8283-2d7dba9a560c.jpg"
                }
            );

            builder.Entity<LexiconEntry>().HasData(
                new LexiconEntry
                {
                    Id = 1,
                    TitleShort = "Eiweiß",
                    TitleLong = "Eiweiß",
                    ImageId = 1,
                    Text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                },
                new LexiconEntry
                {
                    Id = 2,
                    TitleShort = "Proteine",
                    TitleLong = "Proteine",
                    ImageId = 1,
                    Text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                }
            );

            builder.Entity<Ingredient>().HasData(
                new Ingredient
                {
                    Id = 1,
                    Name = "Thunfisch"
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Schokolade"
                },
                new Ingredient
                {
                    Id = 3,
                    Name = "Butter"
                },
                new Ingredient
                {
                    Id = 4,
                    Name = "Kaffee"
                },
                new Ingredient
                {
                    Id = 5,
                    Name = "Käse"
                },
                new Ingredient
                {
                    Id = 6,
                    Name = "Milch"
                },
                new Ingredient
                {
                    Id = 7,
                    Name = "Paprika"
                },
                new Ingredient
                {
                    Id = 8,
                    Name = "Zwiebel"
                },
                new Ingredient
                {
                    Id = 9,
                    Name = "Spinat"
                },
                new Ingredient
                {
                    Id = 10,
                    Name = "Ei"
                },
                new Ingredient
                {
                    Id = 11,
                    Name = "Vollkornbrot"
                },
                new Ingredient
                {
                    Id = 12,
                    Name = "Rind"
                },
                new Ingredient
                {
                    Id = 13,
                    Name = "Schwein"
                },
                new Ingredient
                {
                    Id = 14,
                    Name = "Geflügel"
                },
                new Ingredient
                {
                    Id = 15,
                    Name = "Banane"
                }
            );

            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
                new User
                {
                    Id = "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                    UserName = "test@appsfactory.de",
                    NormalizedUserName = "TEST@APPSFACTORY.DE",
                    Email = "test@appsfactory.de",
                    NormalizedEmail = "TEST@APPSFACTORY.DE",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Wert123+"),
                    SecurityStamp = string.Empty,
                    InsuranceId = 1
                }
            );
        }
    }
}
