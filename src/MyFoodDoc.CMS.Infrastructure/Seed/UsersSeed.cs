using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.CMS.Application.Common;
using MyFoodDoc.CMS.Application.Models;
using System.Linq;

namespace MyFoodDoc.CMS.Infrastructure.Seed
{
    public class UsersSeed : ISeed
    {
        private readonly IHashingManager _hashingManager;
        public UsersSeed(IHashingManager hashingManager)
        {
            this._hashingManager = hashingManager;
        }

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

            if (string.IsNullOrEmpty(context.CmsUsers.FirstOrDefault().PasswordHash))
            {
                string defaultPassword = "Wert123+";
                context.CmsUsers.Where(x => x.Username == "admin" || x.Username == "editor" || x.Username == "editor2" || x.Username == "viewer")
                                .ToList()
                                .ForEach(x => x.PasswordHash = _hashingManager.HashToString(defaultPassword));

                context.SaveChanges();
            }
        }
    }
}