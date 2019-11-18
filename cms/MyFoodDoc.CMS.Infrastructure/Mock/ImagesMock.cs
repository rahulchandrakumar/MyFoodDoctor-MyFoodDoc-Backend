using MyFoodDoc.CMS.Application.Models;
using System.Collections.Generic;

namespace MyFoodDoc.CMS.Infrastructure.Mock
{
    public static class ImagesMock
    {
        public static readonly IList<ImageModel> Default = new List<ImageModel>()
        {
            new ImageModel()
            {
                Id = 0,
                Url = "https://vonoviatestimages.blob.core.windows.net/originals/619464a0-918c-47f6-9f5b-3b535972ba26.jpeg"
            },
            new ImageModel()
            {
                Id = 1,
                Url = "https://vonoviatestimages.blob.core.windows.net/originals/9297a7e8-0d7f-4b2a-ba45-18278e0de546.jpeg"
            }
        };
    }
}
