using System.Collections.Generic;

namespace MyFoodDoc.CMS.Application.Models
{
    public class UserModel: BaseModel<int>
    {
        public string Displayname { get; set; }
        public string Username { get; set; }
        public IEnumerable<UserRoleEnum> Roles { get; set; }
        public string Password { get; set; }
    }
}
