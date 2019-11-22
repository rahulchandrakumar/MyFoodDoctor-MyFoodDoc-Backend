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
    public class LexiconViewModel : BaseEditableListViewModel<LexiconItem, int>
    {
        private readonly ILexiconService _service;

        public LexiconViewModel(ILexiconService service)
        {
            this._service = service;
        }

        protected override Func<Task<IList<LexiconItem>>> GetData => async () =>
        {
            try
            {
                return (await _service.GetItems()).Select(LexiconItem.FromModel).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        };

        public override Action<LexiconItem> Add => async (LexiconItem item) =>
        {
            try
            {
                var itemMod = LexiconItem.FromModel(await _service.AddItem(item.ToModel()));

                this.AddList(itemMod);
            }
            catch(Exception ex) 
            { 
            }
        };
        public override Action<LexiconItem> Update => async (LexiconItem item) =>
        {
            try
            {
                var itemMod = LexiconItem.FromModel(await _service.UpdateItem(item.ToModel()));
                if (itemMod != null)
                {
                    this.UpdateList(itemMod);
                }
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {

            }
        };
    }
}