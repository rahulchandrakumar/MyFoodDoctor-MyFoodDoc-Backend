using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.Mock;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class ImageService : IImageService
    {
        public async Task<ImageModel> AddItem(ImageModel item)
        {
            item.Id = ImagesMock.Default.Count == 0 ? 0 : (ImagesMock.Default.Max(u => u.Id) + 1);

            item.Url = "https://dummyimage.com/900x300/FFF/000ba8&text=Uploaded+(mock)+" + item.Id;
            ImagesMock.Default.Add(item);

            return await Task.FromResult(item);
        }

        public async Task<bool> DeleteItem(int id)
        {
            var user = ImagesMock.Default.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return await Task.FromResult(false);

            ImagesMock.Default.Remove(user);
            return await Task.FromResult(true);
        }

        public async Task<ImageModel> GetItem(int id)
        {
            return await Task.FromResult(ImagesMock.Default.FirstOrDefault(u => u.Id == id));
        }

        public async Task<IList<ImageModel>> GetItems()
        {
            return await Task.FromResult(ImagesMock.Default);
        }

        public async Task<ImageModel> UpdateItem(ImageModel item)
        {
            var user = ImagesMock.Default.FirstOrDefault(u => u.Id == item.Id);

            if (user == null)
                return null;

            ImagesMock.Default.Remove(user);
            ImagesMock.Default.Add(item);
            return await Task.FromResult(item);
        }
    }
}
