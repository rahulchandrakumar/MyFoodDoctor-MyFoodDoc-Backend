using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class UpdatePushNotificationsPayloadValidation : AbstractValidator<UpdatePushNotificationsPayload>
    {
        public UpdatePushNotificationsPayloadValidation()
        {
            RuleFor(x => x.DeviceToken).NotEmpty();
        }
    }
}
