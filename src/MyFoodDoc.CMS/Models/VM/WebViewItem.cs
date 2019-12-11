using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class WebViewItem : ColabDataTableBaseModel<int>
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public bool Undeletable { get; set; }

        public static WebViewItem FromModel(WebViewModel model)
        {
            return new WebViewItem()
            {
                Id = model.Id,
                Title = model.Title,
                Text = model.Text,
                Url = model.Url,
                Undeletable = model.Undeletable
            };
        }

        public WebViewModel ToModel()
        {
            return new WebViewModel()
            {
                Id = this.Id,
                Title = this.Title,
                Text = this.Text,
                Url = this.Url,
                Undeletable = this.Undeletable
            };
        }
    }
}
