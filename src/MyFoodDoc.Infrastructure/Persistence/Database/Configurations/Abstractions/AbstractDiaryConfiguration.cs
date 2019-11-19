using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions
{
    public abstract class AbstractDiaryConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : class, IDiaryEntity<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(e => new { e.UserId, e.Date });
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Date).IsRequired().HasColumnType("Date");

            builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).IsRequired();
        }
    }
}
