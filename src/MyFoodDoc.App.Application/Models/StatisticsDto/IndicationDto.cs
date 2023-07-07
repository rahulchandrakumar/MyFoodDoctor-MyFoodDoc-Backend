using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record IndicationDto(int Id,  IEnumerable<int> TargetIds);