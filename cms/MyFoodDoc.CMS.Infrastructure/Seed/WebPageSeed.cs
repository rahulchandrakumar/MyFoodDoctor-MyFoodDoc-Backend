using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.CMS.Infrastructure.Common;
using System;
using System.IO;
using System.Linq;

namespace MyFoodDoc.CMS.Application.Seed
{
    public class WebPageSeed : ISeed
    {
        public static Uri OriginalUrl { private get; set; }

        public void SeedData(IApplicationContext context)
        {
            if (context.WebPages.Count() == 0)
            {
                context.WebPages.AddRange(new WebPage[]
                {
                    new WebPage()
                    {
                        IsDeletable = false,
                        Text = "",
                        Title = "CSS Styles",
                        Url = new Uri(OriginalUrl, $"{Consts.WebPageContainerName}/{Consts.WebPageCssFilename}").ToString()
                    },
                    new WebPage()
                    {
                        IsDeletable = false,
                        Text = "",
                        Title = "Datenschutz",
                        Url = new Uri(OriginalUrl, $"{Consts.WebPageContainerName}/security-policy.html").ToString()
                    },
                    new WebPage()
                    {
                        IsDeletable = false,
                        Text = "",
                        Title = "Impressum",
                        Url = new Uri(OriginalUrl, $"{Consts.WebPageContainerName}/about.html").ToString()
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
