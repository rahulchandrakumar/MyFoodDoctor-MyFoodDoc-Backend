using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using MyFoodDoc.CMS.Infrastructure.Common;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.AzureBlob.Implementation
{
    public class ImageBlobService : IImageBlobService
    {
        public static string ConnectionString { private get; set; }

        private CloudBlobClient _client = null;
        private CloudBlobContainer _container = null;

        public ImageBlobService()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(ConnectionString);
            _client = account.CreateCloudBlobClient();

            _container = _client.GetContainerReference(Consts.ImagesContainerName);
            _container.CreateIfNotExistsAsync();

            _container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
        }

        public async Task<string> UploadImage(Stream stream, string fileType, string filename = null, CancellationToken cancellationToken = default)
        {
            if (stream == null || !stream.CanRead)
                return null;

            filename = filename ?? (Guid.NewGuid().ToString() + ".jpg");
            CloudBlockBlob blob = _container.GetBlockBlobReference(filename);
            blob.Properties.ContentType = fileType;

            bool imageExists = await blob.ExistsAsync(cancellationToken);
            if (imageExists)
            {
                await blob.DeleteAsync(cancellationToken);
            }
            await blob.UploadFromStreamAsync(stream, cancellationToken);
            return blob.Uri.AbsoluteUri;
        }

        public async Task<bool> DeleteImage(string url, CancellationToken cancellationToken = default)
        {
            var fileName = url.Split('/').LastOrDefault();
            CloudBlockBlob blob = _container.GetBlockBlobReference(fileName);

            bool imageExists = await blob.ExistsAsync(cancellationToken);
            if (imageExists)
            {
                await blob.DeleteAsync(cancellationToken);
                return true;
            }
            return false;
        }
    }
}
