using Microsoft.Extensions.Caching.Memory;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class FileService : IFileService
    {
        private readonly IMemoryCache _memoryCache;
        private const string cachePrefix = nameof(FileService) + "_";
        private const string cacheCounter = cachePrefix + "counter";

        public FileService(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;

            _memoryCache.Set(cacheCounter, 0);
        }

        public async Task<FileModel> GetTempFile(int id, CancellationToken cancellationToken = default)
        {
            var fileData = _memoryCache.Get<byte[]>(cachePrefix + id);

            if (fileData == null)
                return null;

            return await Task.FromResult(new FileModel()
            {
                Id = id,
                Data = fileData
            });
        }

        public async Task<int> StoreTempFile(Stream fileStream, TimeSpan duration, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream())
            {
                await fileStream.CopyToAsync(stream, cancellationToken);
                var fileData = stream.ToArray();

                var key = _memoryCache.Get<int>(cacheCounter);
                _memoryCache.Set(cacheCounter, ++key);

                _memoryCache.Set(cachePrefix + key, fileData, duration);

                return key;
            }
        }
    }
}
