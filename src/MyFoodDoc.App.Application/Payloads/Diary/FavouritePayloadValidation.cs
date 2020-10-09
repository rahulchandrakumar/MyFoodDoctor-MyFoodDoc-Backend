using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class FavouritePayloadValidation : AbstractValidator<FavouritePayload>
    {
        public FavouritePayloadValidation()
        {
            RuleFor(x => x.Title).NotEmpty();

            RuleForEach(x => x.Ingredients).ChildRules(i =>
            {
                i.RuleFor(x => x.FoodId).NotNull();
                i.RuleFor(x => x.ServingId).NotNull();
                i.RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
            });
        }
    }
}
