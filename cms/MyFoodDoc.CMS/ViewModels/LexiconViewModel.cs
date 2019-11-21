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
    public class LexiconViewModel : BaseListViewModel<LexiconItem>
    {
        private readonly ILexiconService _service;

        public LexiconViewModel(ILexiconService service)
        {
            this._service = service;

            //init props
            Init();
        }
        private Action Init => () =>
        {
            try
            {
                this.Items = _service.GetItems().Result.Select(LexiconItem.FromModel).ToList();
            }
            catch (Exception ex)
            {

            }
        };

        public Action<LexiconItem> Add => async (LexiconItem item) =>
        {
            try
            {
                this.AddList(nameof(Items), LexiconItem.FromModel(await _service.AddItem(item.ToModel())));
                this.PushUpdates();
            }
            catch(Exception ex) 
            { 
            }
        };
        public Action<LexiconItem> Update => async (LexiconItem item) =>
        {
            try
            {
                if (await _service.UpdateItem(item.ToModel()) != null)
                {
                    this.UpdateList(nameof(Items), item);
                    this.PushUpdates();
                }
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {

            }
        };
    }
}