using MyFoodDoc.Application.Entites;
using System;

namespace MyFoodDoc.CMS.Application.Models
{
    public class ImageModel: BaseModel
    {
        #region CDN
        public static Uri CDN { private get; set; }
        public static Uri OriginalUrl { private get; set; }

        private static string GetCDNUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            var newUrl = url;
            if (CDN != null)
            {
                newUrl = new Uri(CDN, new Uri(url).LocalPath.ToString()).ToString();
            }
            return newUrl;
        }

        private static string GetOriginalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            var newUrl = new Uri(OriginalUrl, new Uri(url).LocalPath.ToString()).ToString();
            return newUrl;
        }
        #endregion

        public string Url { get; set; }

        public static ImageModel FromEntity(Image entity)
        {
            return new ImageModel()
            {
                Id = entity.Id,
                Url = GetCDNUrl(entity.Url)
            };
        }

        public Image ToEntity()
        {
            return new Image()
            {
                Id = this.Id,
                LastModified = DateTime.Now,
                Url = GetOriginalUrl(this.Url)
            };
        }
    }
}
