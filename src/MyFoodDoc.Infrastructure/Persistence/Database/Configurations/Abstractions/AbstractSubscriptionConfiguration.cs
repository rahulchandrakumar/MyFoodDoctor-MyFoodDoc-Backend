using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Subscriptions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions
{
    public abstract class AbstractSubscriptionConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AbstractSubscription
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        }
    }
}
