using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.Methods;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions;
using System;
using System.Linq;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    /*
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        private readonly ApplicationContext _context;

        public ReportConfiguration(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Reports", "Analysis");
            builder.HasKey(report => report.Id);

            builder.Property(report => report.Id).IsRequired().ValueGeneratedOnAdd();

            //builder.HasMany(report => report.Meals.Query()Where(m => m.Date >= report.StartDate && m.Date <= report.EndDate));

            builder.HasMany(report => report.Meals.Where(meal => meal.Date >= report.StartDate && meal.Date <= report.EndDate));
            builder.HasMany(report => report.Flags).WithOne(flag => flag.Report);
            builder.HasMany(report => report.Methods).WithOne(method => method.Report);
            builder.HasMany(report => report.Targets).WithOne(target => target.Report);
        }
    }
    */
}
