using DotNetify;
using DotNetify.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels
{
    public class IngredientSize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Editor { get; set; }
        public long? LockDate { get; set; }
    }

    [Authorize]
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

        public Action<IngredientSize> Add => (IngredientSize ingredientSize) =>
        {
            ingredientSize.Id = IngredientSizes.Count == 0 ? 0 : IngredientSizes.Max(i => i.Id) + 1;
            IngredientSizes.Add(ingredientSize);

            this.AddList(nameof(IngredientSizes), ingredientSize);
        };
        public Action<IngredientSize> Update => (IngredientSize ingredientSize) =>
        {
            IngredientSizes.Remove(IngredientSizes.First(i => i.Id == ingredientSize.Id));
            IngredientSizes.Add(ingredientSize);

            this.UpdateList(nameof(IngredientSizes), ingredientSize);
        };
        public Action<int> Remove => (int Id) =>
        {
            IngredientSizes.Remove(IngredientSizes.First(i => i.Id == Id));

            this.RemoveList(nameof(IngredientSizes), Id);
        };
    }
}
