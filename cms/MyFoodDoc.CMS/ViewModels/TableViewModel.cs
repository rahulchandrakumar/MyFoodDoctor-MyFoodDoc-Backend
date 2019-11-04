using DotNetify;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels
{
    public class IngredientSize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    public class TableViewModel: MulticastVM
    {
        public string IngredientSizes_itemKey => nameof(IngredientSize.Id);
        public IList<IngredientSize> IngredientSizes = null;

        public TableViewModel()
        {
            //init props
            Task.Factory.StartNew(async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    IngredientSizes = new List<IngredientSize>()
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
                    };
                });
            });
        }

        public Action<IngredientSize> Add => (IngredientSize ingredientSize) => this.AddList(nameof(IngredientSizes), ingredientSize);
        public Action<IngredientSize> Update => (IngredientSize ingredientSize) => this.UpdateList(nameof(IngredientSizes), ingredientSize);
        public Action<IngredientSize> Remove => (IngredientSize ingredientSize) => this.RemoveList(nameof(IngredientSizes), ingredientSize.Id);
    }
}
