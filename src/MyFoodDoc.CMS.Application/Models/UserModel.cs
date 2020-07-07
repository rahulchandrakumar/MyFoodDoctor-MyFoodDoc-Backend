using MyFoodDoc.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.CMS.Application.Models
{
    public class UserModel: BaseModel<int>
    {
        public string Displayname { get; set; }
        public string Username { get; set; }
        public UserRoleEnum Role { get; set; }
        public string PasswordHash { get; set; }

        public IEnumerable<UserRoleEnum> Roles => Enum.GetValues(typeof(UserRoleEnum)).Cast<UserRoleEnum>().Where(x => x <= Role);

        public static UserModel FromEntity(CmsUser entity)
        {
            return entity == null ? null : new UserModel()
            {
                Id = entity.Id,
                Displayname = entity.Displayname,
                Username = entity.Username,
                Role = (UserRoleEnum)entity.Role,
                PasswordHash = entity.PasswordHash
            };
        }

        public CmsUser ToEntity()
        {
            return new CmsUser()
            {
                Id = this.Id,
                Displayname = this.Displayname,
                Username = this.Username,
                Role = (int)this.Role,
                PasswordHash = this.PasswordHash
            };
        }
    }
}
