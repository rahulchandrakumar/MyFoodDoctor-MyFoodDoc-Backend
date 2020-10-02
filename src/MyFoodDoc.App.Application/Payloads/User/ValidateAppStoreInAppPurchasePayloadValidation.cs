using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class ValidateAppStoreInAppPurchasePayloadValidation : AbstractValidator<ValidateAppStoreInAppPurchasePayload>
        {
        public ValidateAppStoreInAppPurchasePayloadValidation()
        {
            RuleFor(x => x.ReceiptData).NotEmpty();
        }
    }
}
