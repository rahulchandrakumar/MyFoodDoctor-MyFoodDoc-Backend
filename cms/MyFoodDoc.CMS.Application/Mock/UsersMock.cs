using MyFoodDoc.CMS.Application.Models;
using System.Collections.Generic;

namespace MyFoodDoc.CMS.Application.Mock
{
    public static class UsersMock
    {
        public static readonly IList<UserModel> Default = new List<UserModel>() 
        {
            new UserModel
            {
                Id = 0,
                Displayname = "Admin Admin",
                Username = "admin",
                Roles = new UserRoleEnum[] { UserRoleEnum.Admin, UserRoleEnum.Editor, UserRoleEnum.Viewer }
            },
            new UserModel
            {
                Id = 1,
                Displayname = "editor",
                Username = "editor",
                Roles = new UserRoleEnum[] { UserRoleEnum.Editor, UserRoleEnum.Viewer }
            },
            new UserModel
            {
                Id = 2,
                Displayname = "editor2",
                Username = "editor2",
                Roles = new UserRoleEnum[] { UserRoleEnum.Editor, UserRoleEnum.Viewer }
            },
            new UserModel
            {
                Id = 3,
                Displayname = "Viewer",
                Username = "viewer",
                Roles = new UserRoleEnum[] { UserRoleEnum.Viewer }
            }
        };
    }
}
