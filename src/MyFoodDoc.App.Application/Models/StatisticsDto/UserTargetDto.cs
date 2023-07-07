using System;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record UserTargetDto(int TargetId, string TargetAnswerCode, DateTime Created, FullUserTargetDto Target);