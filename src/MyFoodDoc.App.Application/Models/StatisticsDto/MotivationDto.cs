using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record MotivationDto(int Id,  IEnumerable<int> TargetIds);