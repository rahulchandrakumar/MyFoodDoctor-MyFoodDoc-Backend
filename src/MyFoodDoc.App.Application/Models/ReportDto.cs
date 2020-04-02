using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFoodDoc.App.Application.Models
{
    public class ReportDto : Dictionary<string, ReportDto.OptimizationAreaData>, IMapFrom<Report>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Report, ReportDto>().ConvertUsing<Converter>();
        }

        public class OptimizationAreaData
        {
            public IEnumerable<HistoryEntry> History { get; set; }

            public IEnumerable<ReportTarget> Targets { get; set; }
        }

        public class Converter : ITypeConverter<Report, ReportDto>
        {
            public ReportDto Convert(Report source, ReportDto destination, ResolutionContext context)
            {
                var result = new ReportDto();
                var optimizationAreas = source.Targets.GroupBy(x => x.Target.OptimizationArea.Key);

                foreach (var area in optimizationAreas)
                {
                    var areaReport = context.Mapper.Map<OptimizationAreaData>(area);

                    // Todo

                    result[area.Key] = null;
                }

                return result;
            }
        }

        public class HistoryEntry
        {
            public DateTime Date { get; set; }

            public decimal Value { get; set; }
        }
    }
}
