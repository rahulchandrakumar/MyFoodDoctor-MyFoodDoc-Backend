using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.CMS.Application.Models;
using System.Linq;

namespace MyFoodDoc.CMS.Infrastructure.Seed
{
    public class UsersSeed : ISeed
    {
        public void SeedData(IApplicationContext context)
        {
            if (context.CmsUsers.Count() == 0)
            {
                context.CmsUsers.AddRange(new CmsUser[] {
                    new CmsUser
                    {
                        Displayname = "Admin Admin",
                        Username = "admin",
                        Role = (int)UserRoleEnum.Admin
                    },
                    new CmsUser
                    {
                        Displayname = "editor",
                        Username = "editor",
                        Role = (int)UserRoleEnum.Editor
                    },
                    new CmsUser
                    {
                        Displayname = "editor2",
                        Username = "editor2",
                        Role = (int)UserRoleEnum.Editor
                    },
                    new CmsUser
                    {
                        Displayname = "Viewer",
                        Username = "viewer",
                        Role = (int)UserRoleEnum.Viewer
                    }
                });

                context.SaveChanges();
            }
        }
    }
}