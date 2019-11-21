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
