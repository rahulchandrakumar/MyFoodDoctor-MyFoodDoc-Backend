using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class ResetPasswordPayloadValidation : AbstractValidator<ResetPasswordPayload>
    {
        public ResetPasswordPayloadValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.ResetToken).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }
}
