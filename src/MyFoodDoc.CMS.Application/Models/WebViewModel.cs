using MyFoodDoc.Application.Entities;
using MyFoodDoc.CMS.Application.Common;
using System;

namespace MyFoodDoc.CMS.Application.Models
{
    public class WebViewModel : BaseModel<int>
    {
        #region CDN
        public static Uri CdnUrl { private get; set; }
        public static Uri OriginalUrl { private get; set; }
        #endregion

        public string Title { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public bool Undeletable { get; set; }

        public static WebViewModel FromEntity(WebPage entity)
        {
            return entity == null ? null : new WebViewModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Text = entity.Text,
                Url = CDN.GetCDNUrl(entity.Url, CdnUrl),
                Undeletable = !entity.IsDeletable
            };
        }

        public WebPage ToEntity()
        {
            return new WebPage()
            {
                Id = this.Id,
                Title = this.Title,
                Text = this.Text,
                Url = CDN.GetOriginalUrl(this.Url, OriginalUrl),
                IsDeletable = !this.Undeletable
            };
        }
    }
}
