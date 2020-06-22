using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Methods;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions
{
    /*
    public class ChoiceMethodConfiguration : MethodConfiguration<ChoiceMethod>
    {
        public override void Configure(EntityTypeBuilder<ChoiceMethod> builder)
        {
            base.Configure(builder);

            builder.ToTable("ChoiceMethods", "Analysis");
            builder.HasDiscriminator(method => method.Type);

            builder.HasMany(method => method.Choices).WithOne(method => method.Method).HasForeignKey(method => method.MethodId).OnDelete(DeleteBehavior.Cascade);
        }
    }
    */
}
