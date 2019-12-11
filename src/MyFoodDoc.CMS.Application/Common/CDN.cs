using System;

namespace MyFoodDoc.CMS.Application.Common
{
    internal static class CDN
    {
        internal static string GetCDNUrl(string url, Uri CDN)
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

        internal static string GetOriginalUrl(string url, Uri OriginalUrl)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            var newUrl = new Uri(OriginalUrl, new Uri(url).LocalPath.ToString()).ToString();
            return newUrl;
        }
    }
}
