using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites.Methods;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions
{
    /*
    public abstract class ValueMethodConfiguration : MethodConfiguration<ValueMethod>
    {
        public override void Configure(EntityTypeBuilder<ValueMethod> builder)
        {
            base.Configure(builder);

            builder.ToTable("ValueMethods", "Analysis");
            builder.HasDiscriminator(method => method.Type);
        }
    }
    */
}
