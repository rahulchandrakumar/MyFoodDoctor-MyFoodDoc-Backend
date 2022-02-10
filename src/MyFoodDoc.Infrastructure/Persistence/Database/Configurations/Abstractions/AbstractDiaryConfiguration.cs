using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions
{
    public abstract class AbstractDiaryConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : class, IDiaryEntity<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Date).IsRequired().HasColumnType("Date");

            builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).IsRequired();
        }
    }
}
