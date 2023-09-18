using System;
using System.Linq;
using System.Linq.Expressions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Extensions;

public static class UserExpressions
{
    public static Expression<Func<User, StatisticsUserDto>> Selector(string userId)
    {
        return x => new StatisticsUserDto
        {
            Created = x.Created,
            Email = x.Email,
            Age = x.Birthday == null ? null : DateTime.UtcNow.Year - x.Birthday.Value.Year,
            Gender = x.Gender,
            Height = x.Height,
            HasZPPSubscription =
                x.AppStoreSubscriptions
                    .Any(sub => sub.UserId == userId && sub.Type == SubscriptionType.ZPP && sub.IsValid)
                ||
                x.GooglePlayStoreSubscriptions
                    .Any(sub => sub.UserId == userId && sub.Type == SubscriptionType.ZPP && sub.IsValid),
            HasSubscription =
                x.AppStoreSubscriptions
                    .Any(sub => sub.UserId == userId && (sub.Type == SubscriptionType.ZPP || sub.Type == SubscriptionType.MyFoodDoc) && sub.IsValid)
                ||
                x.GooglePlayStoreSubscriptions
                    .Any(sub => sub.UserId == userId && (sub.Type == SubscriptionType.ZPP || sub.Type == SubscriptionType.MyFoodDoc) && sub.IsValid),
            InsuranceId = x.InsuranceId,
            MotivationTargets = x.Motivations
                .SelectMany(m => m.Motivation.Targets.Where(t => t.MotivationId == m.MotivationId)
                    .Select(mt => new TargetDetailsDto
                    {
                        Id = mt.Target.Id,
                        OptimizationAreaId = mt.Target.OptimizationAreaId,
                        TriggerOperator = mt.Target.TriggerOperator,
                        TriggerValue = mt.Target.TriggerValue,
                        Threshold = mt.Target.Threshold,
                        Priority = mt.Target.Priority,
                        Title = mt.Target.Title,
                        Text = mt.Target.Text,
                        Type = mt.Target.Type,
                        ImageId = mt.Target.ImageId,
                        OptimizationArea = mt.Target.OptimizationArea.ToOptimizationAreaTargetDto(),
                        AdjustmentTarget = mt.Target.AdjustmentTargets.Select(at => at.ToAdjustmentTargetDto()).ToList()
                    })),
            IndicationTargets = x.Indications
                .SelectMany(i => i.Indication.Targets.Where(t => t.IndicationId == i.IndicationId)
                    .Select(it => new TargetDetailsDto
                    {
                        Id = it.TargetId,
                        OptimizationAreaId = it.Target.OptimizationAreaId,
                        TriggerOperator = it.Target.TriggerOperator,
                        TriggerValue = it.Target.TriggerValue,
                        Threshold = it.Target.Threshold,
                        Priority = it.Target.Priority,
                        Title = it.Target.Title,
                        Text = it.Target.Text,
                        Type = it.Target.Type,
                        ImageId = it.Target.ImageId,
                        OptimizationArea = it.Target.OptimizationArea.ToOptimizationAreaTargetDto(),
                        AdjustmentTarget = it.Target.AdjustmentTargets.Select(at => at.ToAdjustmentTargetDto()).ToList()
                    })),
            Diets = x.Diets.Where(userDiet => userDiet.UserId == userId).Select(d => new DietDto(
                d.DietId,
                d.Diet.Key,
                d.Diet.Targets.Where(target => target.DietId == d.DietId)
                    .Select(dt => new TargetDetailsDto
                    {
                        Id = dt.TargetId,
                        OptimizationAreaId = dt.Target.OptimizationAreaId,
                        TriggerOperator = dt.Target.TriggerOperator,
                        TriggerValue = dt.Target.TriggerValue,
                        Threshold = dt.Target.Threshold,
                        Priority = dt.Target.Priority,
                        Title = dt.Target.Title,
                        Text = dt.Target.Text,
                        Type = dt.Target.Type,
                        ImageId = dt.Target.ImageId,
                        OptimizationArea = dt.Target.OptimizationArea.ToOptimizationAreaTargetDto(),
                        AdjustmentTarget = dt.Target.AdjustmentTargets.Select(at => at.ToAdjustmentTargetDto()).ToList()
                    })
            )).ToList(),
            Weights = x.WeightHistory.Where(weight => weight.UserId == userId).Select(w => new UserWeightDto(
                w.Date,
                w.Value
            )).ToList()
        };
    }
}