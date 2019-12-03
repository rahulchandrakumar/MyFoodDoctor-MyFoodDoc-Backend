using MyFoodDoc.CMS.Application.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IFileService
    {
        Task<int> StoreTempFile(Stream fileStream, TimeSpan duration, CancellationToken cancellationToken = default);
        Task<FileModel> GetTempFile(int id, CancellationToken cancellationToken = default);
    }
}
