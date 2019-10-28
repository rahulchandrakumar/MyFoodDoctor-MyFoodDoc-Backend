using Microsoft.AspNetCore.Identity;

namespace MyFoodDoc.Application.Entites
{
    public class UserIdentity : IdentityUser<int>
    {
        public virtual User Profile { get; }
    }
}
