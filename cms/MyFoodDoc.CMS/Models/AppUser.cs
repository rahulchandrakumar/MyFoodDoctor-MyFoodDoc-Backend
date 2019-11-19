using System.Collections.Generic;

namespace MyFoodDoc.CMS.Models
{
    public class AppUser
    {
        public string Displayname { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Token { get; set; }
    }
}
