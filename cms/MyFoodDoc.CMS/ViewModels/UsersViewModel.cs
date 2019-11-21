using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace MyFoodDoc.CMS.ViewModels
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

    [Authorize("Admin")]
    public class UsersViewModel : BaseListViewModel<User>
    {
        private readonly IUserService _service;

        public UsersViewModel(IUserService userService)
        {
            this._service = userService;

            //init props
            Init();
        }

        private Action Init => () =>
        {
            try
            {
                this.Items = _service.GetItems().Result.Select(User.FromModel).ToList();
            }
            catch (Exception ex)
            {

            }
        };

        public Action<User> Add => async (User user) =>
        {
            try
            {
                this.AddList(nameof(Items), User.FromModel(await _service.AddItem(user.ToModel())));
                this.PushUpdates();
            }
            catch (Exception ex)
            {

            }
        };
        public Action<User> Update => async (User user) =>
        {
            try
            {
                if (await _service.UpdateItem(user.ToModel()) != null)
                {
                    this.UpdateList(nameof(Items), user);
                    this.PushUpdates();
                }
            }
            catch (Exception ex)
            {

            }
        };
        public Action<int> Remove => async (int Id) =>
        {
            try
            {
                if (await _service.DeleteItem(Id))
                {
                    this.RemoveList(nameof(Items), Id);
                    this.PushUpdates();
                }
            }
            catch (Exception ex)
            {

            }
        };
    }
}
