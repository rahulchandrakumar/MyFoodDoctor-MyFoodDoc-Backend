using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;
using System;
using System.Linq;

namespace MyFoodDoc.CMS.Models.VM
{
    public class User : ColabDataTableBaseModel
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public static User FromModel(UserModel model)
        {
            return new User()
            {
                DisplayName = model.Displayname,
                Id = model.Id,
                Password = model.Password,
                Role = model.Roles.Max().ToString(),
                Username = model.Username
            };
        }

        public UserModel ToModel()
        {
            return new UserModel()
            {
                Displayname = this.DisplayName,
                Id = this.Id,
                Password = this.Password,
                Username = this.Username,
                Roles = Enum.GetValues(typeof(UserRoleEnum)).Cast<UserRoleEnum>().Where(x => x <= Enum.Parse<UserRoleEnum>(this.Role))
            };
        }
    }
}
