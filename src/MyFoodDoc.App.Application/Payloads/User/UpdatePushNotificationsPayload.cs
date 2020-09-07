using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class UpdatePushNotificationsPayload
    {
        public bool IsNotificationsEnabled { get; set; }

        public string DeviceToken { get; set; }
    }
}
