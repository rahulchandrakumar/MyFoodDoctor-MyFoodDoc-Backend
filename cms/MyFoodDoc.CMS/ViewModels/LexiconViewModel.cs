using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace MyFoodDoc.CMS.ViewModels
{

    [Authorize("Admin")]
    public class LexiconViewModel : MulticastVM
    {
        public class LexiconItem : ColabDataTableBaseModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public Image Image { get; set; }

            public static LexiconItem FromModel(LexiconModel model)
            {
                return new LexiconItem()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Image = Image.FromModel(model.Image)
                };
            }

            public LexiconModel ToModel()
            {
                return new LexiconModel()
                {
                    Id = this.Id,
                    Title = this.Title,
                    Description = this.Description,
                    Image = this.Image.ToModel()
                };
            }
        }

        public string Items_itemKey => nameof(LexiconItem.Id);
        public IList<LexiconItem> Items = new List<LexiconItem>();

        private readonly ILexiconService _service;

        public LexiconViewModel(ILexiconService service)
        {
            this._service = service;
            //init props
            Observable.FromAsync(async () => (await _service.GetItems()).Select(LexiconItem.FromModel).ToList())
                      .Subscribe(x =>
                      {
                          Items = x;
                          PushUpdates();
                      });
        }

        public Action<LexiconItem> Add => async (LexiconItem item) =>
        {
            var itemModel = item.ToModel();
            itemModel = await _service.AddItem(itemModel);

            var intUser = LexiconItem.FromModel(itemModel);

            Items.Add(intUser);
            this.AddList(nameof(Items), intUser);
        };
        public Action<LexiconItem> Update => async (LexiconItem item) =>
        {
            if (await _service.UpdateItem(item.ToModel()) != null)
            {
                Items.Remove(Items.First(i => i.Id == item.Id));
                Items.Add(item);

                this.UpdateList(nameof(Items), item);
            }
        };
        public Action<int> Remove => async (int Id) =>
        {
            if (await _service.DeleteItem(Id))
            {
                Items.Remove(Items.First(i => i.Id == Id));
                this.RemoveList(nameof(Items), Id);
            }
        };
    }
}