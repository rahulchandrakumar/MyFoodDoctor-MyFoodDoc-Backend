using MyFoodDoc.App.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Mock
{
    public static class UserMock
    {
        public static UserDto Default = new UserDto
        {
            Email = "test@appsfactory.de",
        };
    }
}
