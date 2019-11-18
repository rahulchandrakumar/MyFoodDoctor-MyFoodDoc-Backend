using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize("Editor")]
    public class TableViewModel: MulticastVM
    {
        public class IngredientSize : ColabDataTableBaseModel
        {
            public string Name { get; set; }
            public decimal Amount { get; set; }
        }

        public string Items_itemKey => nameof(IngredientSize.Id);
        public IList<IngredientSize> Items = new List<IngredientSize>()
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
        
        public Action<IngredientSize> Add => (IngredientSize ingredientSize) =>
        {
            ingredientSize.Id = Items.Count == 0 ? 0 : Items.Max(i => i.Id) + 1;
            Items.Add(ingredientSize);

            this.AddList(nameof(Items), ingredientSize);
        };
        public Action<IngredientSize> Update => (IngredientSize ingredientSize) =>
        {
            Items.Remove(Items.First(i => i.Id == ingredientSize.Id));
            Items.Add(ingredientSize);

            this.UpdateList(nameof(Items), ingredientSize);
        };
        public Action<int> Remove => (int Id) =>
        {
            Items.Remove(Items.First(i => i.Id == Id));

            this.RemoveList(nameof(Items), Id);
        };
    }
}
