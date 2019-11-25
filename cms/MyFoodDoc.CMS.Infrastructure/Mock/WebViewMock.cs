using MyFoodDoc.CMS.Application.Models;
using System.Collections.Generic;

namespace MyFoodDoc.CMS.Infrastructure.Mock
{
    public static class WebViewMock
    {
        public static readonly IList<WebViewModel> Default = new List<WebViewModel>()
        {
            new WebViewModel()
            {
                Id = 1,
                Title = "Privacy policy",
                Text = "Privacy policy",
                Undeletable = true
            },
            new WebViewModel()
            {
                Id = 2,
                Title = "About us",
                Text = "About us",
                Undeletable = true
            },
            new WebViewModel()
            {
                Id = 3,
                Title = "Deletable",
                Text = "Deletable",
            }
        };
    }
}
