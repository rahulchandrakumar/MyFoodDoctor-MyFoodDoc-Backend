using DotNetify;
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
    /*[Authorize("Editor")]
    public class PortionsViewModel : BaseEditableListViewModel<Ingredient, int>
    {
        private IIngredientService _service;
        public PortionsViewModel(IIngredientService ingredientService, IConnectionContext connectionContext) : base(connectionContext)
        {
            this._service = ingredientService;
        }

        protected override Func<Task<IList<Ingredient>>> GetData => async () =>
        {
            return (await _service.GetItems()).Select(Ingredient.FromModel).ToList();
        };

        public override Action<Ingredient> Add => async (Ingredient item) =>
        {
            try
            {
                var itemMod = Ingredient.FromModel(await _service.AddItem(item.ToModel()));

                this.AddList(itemMod);
            }
            catch (Exception ex)
            {
                SendError(ex);
            }
        };
        public override Action<Ingredient> Update => async (Ingredient item) =>
        {
            try
            {
                var itemMod = Ingredient.FromModel(await _service.UpdateItem(item.ToModel()));
                if (itemMod != null)
                {
                    this.UpdateList(itemMod);
                }
            }
            catch (Exception ex)
            {
                SendError(ex);
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
                SendError(ex);
            }
        };
    }*/
}
