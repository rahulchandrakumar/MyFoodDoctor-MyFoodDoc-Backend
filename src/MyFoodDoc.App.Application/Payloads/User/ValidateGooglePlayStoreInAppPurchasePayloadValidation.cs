﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class ValidateGooglePlayStoreInAppPurchasePayloadValidation : AbstractValidator<ValidateGooglePlayStoreInAppPurchasePayload>
    {
        public ValidateGooglePlayStoreInAppPurchasePayloadValidation()
        {
            RuleFor(x => x.PurchaseToken).NotEmpty();
        }
    }
}