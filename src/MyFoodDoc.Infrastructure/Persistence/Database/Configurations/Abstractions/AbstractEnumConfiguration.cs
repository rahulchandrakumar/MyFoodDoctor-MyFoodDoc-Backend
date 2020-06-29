using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions
{
    public abstract class AbstractEnumConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEnumEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.Key);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Key).IsRequired().HasMaxLength(50);
            builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
        }
    }
}
