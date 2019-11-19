using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.AzureBlob.Implementation
{
    public class ImageBlobService : IImageBlobService
    {
        public static string ConnectionString { private get; set; }
        public static string ContainerName { private get; set; }
        public static Uri CDN { private get; set; }
        public static Uri OriginalUrl { private get; set; }

        private CloudBlobClient _client = null;
        private CloudBlobContainer _container = null;
        
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

        public ImageBlobService(/*IConfiguration configuration*/)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(ConnectionString);
            _client = account.CreateCloudBlobClient();

            _container = _client.GetContainerReference(ContainerName);
            _container.CreateIfNotExistsAsync();

            _container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
        }

        public async Task<string> UploadImage(byte[] data, string fileType, string filename = null)
        {
            if (data == null)
                return null;

            using (var stream = new MemoryStream(data))
            {
                filename = filename ?? (Guid.NewGuid().ToString() + ".jpg");
                CloudBlockBlob blob = _container.GetBlockBlobReference(filename);
                blob.Properties.ContentType = fileType;

                bool imageExists = await blob.ExistsAsync();
                if (imageExists)
                {
                    await blob.DeleteAsync();
                }
                await blob.UploadFromStreamAsync(stream);
                return GetCDNUrl(blob.Uri.AbsoluteUri);
            }
        }

        public async Task<bool> DeleteImage(string url)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(GetOriginalUrl(url));

            bool imageExists = await blob.ExistsAsync();
            if (imageExists)
            {
                await blob.DeleteAsync();
                return true;
            }
            return false;
        }
    }
}
