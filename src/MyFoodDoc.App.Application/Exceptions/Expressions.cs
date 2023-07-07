using System;
using System.Linq;
using System.Linq.Expressions;
using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Exceptions;

public static class Expressions
{
    public static Expression<Func<User, StatisticsUserDto>> Selector(string userId)
    {
        return x => new StatisticsUserDto()
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
            Motivations = x.Motivations.SelectMany(m => m.Motivation.Targets.Where(t => t.MotivationId == m.MotivationId).Select(mt => mt.TargetId)),
            Indications = x.Indications.SelectMany(i => i.Indication.Targets.Where(t => t.IndicationId == i.IndicationId).Select(it => it.TargetId)),
            Diets = x.Diets.Where(userDiet => userDiet.UserId == userId).Select(d => new DietDto(
                d.DietId,
                d.Diet.Key,
                d.Diet.Targets.Where(target => target.DietId == d.DietId).Select(dt => dt.TargetId)
            )).ToList(),
            Weights = x.WeightHistory.Where(weight => weight.UserId == userId).Select(w => new UserWeightDto(
                w.Date,
                w.Value
            )).ToList()
        };
    }
}