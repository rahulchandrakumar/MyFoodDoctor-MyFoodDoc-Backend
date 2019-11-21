using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Models;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace MyFoodDoc.CMS.ViewModels
{
    public class IngredientSize : ColabDataTableBaseModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    [Authorize("Editor")]
    public class TableViewModel : BaseListViewModel<IngredientSize>
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
                Items.Add(ingredientSize);

                this.AddList(nameof(Items), ingredientSize);
            }
            catch (Exception ex)
            {

            }
        };
        public Action<IngredientSize> Update => (IngredientSize ingredientSize) =>
        {
            try
            {
                Items.Remove(Items.First(i => i.Id == ingredientSize.Id));
                Items.Add(ingredientSize);

                this.UpdateList(nameof(Items), ingredientSize);
            }
            catch (Exception ex)
            {

            }
        };
        public Action<int> Remove => (int Id) =>
        {
            try
            {
                Items.Remove(Items.First(i => i.Id == Id));

                this.RemoveList(nameof(Items), Id);
            }
            catch (Exception ex)
            {

            }
        };
    }
}
