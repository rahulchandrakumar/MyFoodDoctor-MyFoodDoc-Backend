using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models.VM;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize("Admin")]
    public class UsersViewModel : BaseListViewModel<User, int>
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
                var itemMod = User.FromModel(await _service.AddItem(user.ToModel()));

                this.AddList(itemMod);
            }
            catch (Exception ex)
            {

            }
        };
        public Action<User> Update => async (User item) =>
        {
            try
            {
                var itemMod = User.FromModel(await _service.UpdateItem(item.ToModel()));
                if (itemMod != null)
                {
                    this.UpdateList(itemMod);
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
                    this.RemoveList(Id);
                }
            }
            catch (Exception ex)
            {

            }
        };
    }
}
