using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize("Admin")]
    public class UsersViewModel: MulticastVM
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

        public string Items_itemKey => nameof(User.Id);
        public IList<User> Items = null;

        private readonly IUserService _userService;

        public UsersViewModel(IUserService userService)
        {
            this._userService = userService;
            //init props
            Observable.FromAsync(async () => (await _userService.GetItems()).Select(User.FromModel).ToList())
                      .Subscribe(x =>
                      {
                          Items = x;
                          PushUpdates();
                      });
        }

        public Action<User> Add => async (User user) =>
        {
            var userModel = user.ToModel();
            userModel = await _userService.AddItem(userModel);

            var intUser = User.FromModel(userModel);

            Items.Add(intUser);
            this.AddList(nameof(Items), intUser);
        };
        public Action<User> Update => async (User user) =>
        {
            if (await _userService.UpdateItem(user.ToModel()) != null)
            {
                Items.Remove(Items.First(i => i.Id == user.Id));
                Items.Add(user);

                this.UpdateList(nameof(Items), user);
            }
        };
        public Action<int> Remove => async (int Id) =>
        {
            if (await _userService.DeleteItem(Id))
            {
                Items.Remove(Items.First(i => i.Id == Id));
                this.RemoveList(nameof(Items), Id);
            }
        };
    }
}
