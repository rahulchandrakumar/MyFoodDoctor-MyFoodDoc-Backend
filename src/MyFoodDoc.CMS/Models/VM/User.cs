using MyFoodDoc.CMS.Application.Models;
using System;

namespace MyFoodDoc.CMS.Models.VM
{
    public class User : VMBase.BaseModel<int>
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public static User FromModel(UserModel model)
        {
            return model == null ? null : new User()
            {
                DisplayName = model.Displayname,
                Id = model.Id,
                Role = model.Role.ToString(),
                Username = model.Username
            };
        }

        public UserModel ToModel()
        {
            return new UserModel()
            {
                Displayname = this.DisplayName,
                Id = this.Id,
                Username = this.Username,
                Role = Enum.Parse<UserRoleEnum>(this.Role)
            };
        }
    }
}
