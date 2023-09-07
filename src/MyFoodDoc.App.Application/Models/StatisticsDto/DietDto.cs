using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record DietDto(int Id, string Key, IEnumerable<TargetDetailsDto> Targets);