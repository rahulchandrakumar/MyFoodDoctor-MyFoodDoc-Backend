using DotNetify.Security;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Auth;
using MyFoodDoc.CMS.Models.VM;
using MyFoodDoc.CMS.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize("Admin")]
    public class UsersViewModel : BaseEditableListViewModel<User, int>
    {
        private readonly IUserService _service;
        private readonly IHashingManager _hashingManager;

        public UsersViewModel(IUserService userService, IHashingManager hashingManager)
        {
            this._service = userService;
            this._hashingManager = hashingManager;
        }

        protected override Func<Task<IList<User>>> GetData => async () =>
        {
            try
            {
                return (await _service.GetItems()).Select(User.FromModel).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        };

        public override Action<User> Add => async (User item) =>
        {
            try
            {
                var itemIntModel = item.ToModel();
                itemIntModel.PasswordHash = _hashingManager.HashToString(item.Password);

                var itemMod = User.FromModel(await _service.AddItem(itemIntModel));

                this.AddList(itemMod);
            }
            catch (Exception ex)
            {

            }
        };
        public override Action<User> Update => async (User item) =>
        {
            try
            {
                var itemIntModel = item.ToModel();
                itemIntModel.PasswordHash = item.Password == null ? null : _hashingManager.HashToString(item.Password);

                var itemMod = User.FromModel(await _service.UpdateItem(itemIntModel));
                if (itemMod != null)
                {
                    this.UpdateList(itemMod);
                }
            }
            catch (Exception ex)
            {

            }
        };
        public override Action<int> Remove => async (int Id) =>
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
