using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class CmsUser : AbstractAuditableEntity
    {
        public string Displayname { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int Role { get; set; }        
    }
}
