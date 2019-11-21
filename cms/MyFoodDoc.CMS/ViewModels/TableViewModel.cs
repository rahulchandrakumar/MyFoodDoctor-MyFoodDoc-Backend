using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Models.VM;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize("Editor")]
    public class TableViewModel : BaseListViewModel<IngredientSize, int>
    {       
        public TableViewModel()
        {
            Init();
        }
        public Action Init => () =>
        {
            try
            {
                this.Items.Add(new IngredientSize()
                {
                    Id = 0,
                    Name = "Banana",
                    Amount = 100
                });
                this.Items.Add(new IngredientSize()
                {
                    Id = 1,
                    Name = "Apple",
                    Amount = 200
                });
            }
            catch(Exception ex)
            {

            }
        };        

        public Action<IngredientSize> Add => (IngredientSize ingredientSize) =>
        {
            try
            {
                ingredientSize.Id = Items.Count == 0 ? 0 : Items.Max(i => i.Id) + 1;
                
                this.AddList(ingredientSize);
            }
            catch (Exception ex)
            {

            }
        };
        public Action<IngredientSize> Update => (IngredientSize ingredientSize) =>
        {
            try
            {
                this.UpdateList(ingredientSize);
            }
            catch (Exception ex)
            {

            }
        };
        public Action<int> Remove => (int Id) =>
        {
            try
            {
                this.RemoveList(Id);
            }
            catch (Exception ex)
            {

            }
        };
    }
}
