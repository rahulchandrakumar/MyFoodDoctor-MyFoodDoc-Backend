using DotNetify.Security;
using MyFoodDoc.CMS.Application.Persistence;
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
    public class WebViewViewModel : BaseEditableListViewModel<WebViewItem, int>
    {
        private readonly IWebViewService _service;
        public WebViewViewModel(IWebViewService service)
        {
            this._service = service;
        }

        protected override Func<Task<IList<WebViewItem>>> GetData => async () =>
        {
            try
            {
                return (await _service.GetItems()).Select(WebViewItem.FromModel).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        };

        public override Action<WebViewItem> Add => async (WebViewItem user) =>
        {
            try
            {
                var itemMod = WebViewItem.FromModel(await _service.AddItem(user.ToModel()));

                this.AddList(itemMod);
            }
            catch (Exception ex)
            {

            }
        };
        public override Action<WebViewItem> Update => async (WebViewItem item) =>
        {
            try
            {
                var itemMod = WebViewItem.FromModel(await _service.UpdateItem(item.ToModel()));
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
