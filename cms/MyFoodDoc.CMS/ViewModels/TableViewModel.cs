using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Models.VM;
using MyFoodDoc.CMS.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize("Editor")]
    public class TableViewModel : BaseEditableListViewModel<IngredientSize, int>
    {       
        public TableViewModel(IConnectionContext connectionContext): base(connectionContext)
        {
        }
        protected override Func<Task<IList<IngredientSize>>> GetData => async () =>
        {
            return await Task.FromResult(new List<IngredientSize>()
            {
                new IngredientSize()
                {
                    Id = 0,
                    Name = "Banana",
                    Amount = 100
                },
                new IngredientSize()
                {
                    Id = 1,
                    Name = "Apple",
                    Amount = 200
                }
            });
        };     

        public override Action<IngredientSize> Add => (IngredientSize ingredientSize) =>
        {
            try
            {
                ingredientSize.Id = Items.Count == 0 ? 0 : Items.Max(i => i.Id) + 1;
                
                this.AddList(ingredientSize);
            }
            catch (Exception ex)
            {
                SendError(ex);
            }
        };
        public override Action<IngredientSize> Update => (IngredientSize ingredientSize) =>
        {
            try
            {
                this.UpdateList(ingredientSize);
            }
            catch (Exception ex)
            {
                SendError(ex);
            }
        };
        public override Action<int> Remove => (int Id) =>
        {
            try
            {
                this.RemoveList(Id);
            }
            catch (Exception ex)
            {
                SendError(ex);
            }
        };
    }
}
