using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize]
    public class UsersViewModel: MulticastVM
    {
        public class User : ColabDataTableBaseModel
        {
            public string Username { get; set; }
            public string DisplayName { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        public string Items_itemKey => nameof(User.Id);
        public IList<User> Items = null;

        public UsersViewModel()
        {
            //init props
            Task.Factory.StartNew(async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    Items = new List<User>()
                    {
                        new User()
                        {
                            Id = 0,
                            Username = "admin",
                            DisplayName = "Admin",
                            Password = "1234",
                            Role = "Admin"
                        },
                        new User()
                        {
                            Id = 1,
                            Username = "editor",
                            DisplayName = "Editor",
                            Password = "1234",
                            Role = "Editor"
                        },
                        new User()
                        {
                            Id = 2,
                            Username = "viewer",
                            DisplayName = "Viewer",
                            Password = "1234",
                            Role = "Viewer"
                        }
                    };
                });
            });
        }

        public Action<User> Add => (User user) =>
        {
            user.Id = Items.Count == 0 ? 0 : Items.Max(i => i.Id) + 1;
            Items.Add(user);

            this.AddList(nameof(Items), user);
        };
        public Action<User> Update => (User user) =>
        {
            Items.Remove(Items.First(i => i.Id == user.Id));
            Items.Add(user);

            this.UpdateList(nameof(Items), user);
        };
        public Action<int> Remove => (int Id) =>
        {
            Items.Remove(Items.First(i => i.Id == Id));

            this.RemoveList(nameof(Items), Id);
        };
    }
}
