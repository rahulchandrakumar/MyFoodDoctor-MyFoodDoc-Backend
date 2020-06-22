using MyFoodDoc.Application.Entities;
using MyFoodDoc.CMS.Application.Common;
using System;

namespace MyFoodDoc.CMS.Application.Models
{
    public class ImageModel: BaseModel<int>
    {
        #region CDN
        public static Uri CdnUrl { private get; set; }
        public static Uri OriginalUrl { private get; set; }
        #endregion

        public string Url { get; set; }

        public static ImageModel FromEntity(Image entity)
        {
            return entity == null ? null : new ImageModel()
            {
                Id = entity.Id,
                Url = CDN.GetCDNUrl(entity.Url, CdnUrl)
            };
        }

        public Image ToEntity()
        {
            return new Image()
            {
                Id = this.Id,
                LastModified = DateTime.Now,
                Url = CDN.GetOriginalUrl(this.Url, OriginalUrl)
            };
        }
    }
}
