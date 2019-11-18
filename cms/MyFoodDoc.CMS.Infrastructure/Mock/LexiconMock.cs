using MyFoodDoc.CMS.Application.Models;
using System.Collections.Generic;

namespace MyFoodDoc.CMS.Infrastructure.Mock
{
    public static class LexiconMock
    {
        public static readonly IList<LexiconModel> Default = new List<LexiconModel>()
        {
            new LexiconModel()
            {
                Id = 0,
                Title = "Title0",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut nec est massa. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Integer facilisis justo ut placerat sollicitudin. Maecenas ullamcorper nunc sapien, vel finibus risus lacinia sagittis. Nam condimentum, arcu ut consequat lobortis, lectus libero luctus justo, ac suscipit nisi est eget turpis. Aliquam sem leo, imperdiet eget lorem eget, pretium dapibus augue. Praesent laoreet mollis pharetra. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Duis dignissim commodo purus a pharetra. Etiam mollis, eros tincidunt imperdiet elementum, massa eros commodo arcu, in ullamcorper magna elit vel arcu. Curabitur iaculis, lectus non iaculis convallis, libero nisi pretium ex, vel imperdiet sem ante at lorem. Morbi arcu turpis, porta nec lorem ac, convallis posuere tellus. Vivamus ac auctor est. In in turpis ullamcorper, bibendum nibh rutrum, volutpat quam. Duis blandit mollis metus. Aliquam pharetra quis elit sit amet eleifend.",
                Image = ImagesMock.Default[0]
            },
            new LexiconModel()
            {
                Id = 1,
                Title = "Title1",
                Description = "Description1",
                Image = ImagesMock.Default[1]
            }
        };
    }
}
