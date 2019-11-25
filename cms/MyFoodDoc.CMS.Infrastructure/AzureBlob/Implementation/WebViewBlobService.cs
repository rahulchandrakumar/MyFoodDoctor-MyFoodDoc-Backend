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
    public class WebViewBlobService : IWebViewBlobService
    {
        public static string ConnectionString { private get; set; }

        private CloudBlobClient _client = null;
        private CloudBlobContainer _container = null;

        private const string htmlTemplate = "<!DOCTYPE html>\n<html><head><link rel=\"stylesheet\" href=\"{0}\"></head><body>{1}</body></html>";

        public WebViewBlobService()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(ConnectionString);
            _client = account.CreateCloudBlobClient();

            _container = _client.GetContainerReference(Consts.WebPageContainerName);
            _container.CreateIfNotExistsAsync();

            _container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
        }

        private string GetPath(string filename)
        {
            return new Uri(CloudStorageAccount.Parse(ConnectionString).BlobStorageUri.PrimaryUri, $"{Consts.WebPageContainerName}/{filename}").ToString();
        }

        public async Task<bool> UpdateFile(string content, string url, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(url))
                return false;

            string filename = url.Split("/").Last();
            string fileType = $"text/{filename.Split('.').Last()}";

            if (fileType == "text/html") //apply template
            {
                content = String.Format(htmlTemplate, GetPath(Consts.WebPageCssFilename), content);
            }
            else if (fileType == "text/css") //upload as is
            { }
            else
            {
                throw new ArgumentException("Invalid argument value. Supported values: text/html and text/css", nameof(fileType));
            }

            CloudBlockBlob blob = _container.GetBlockBlobReference(filename);
            blob.Properties.ContentType = fileType;

            bool imageExists = await blob.ExistsAsync(cancellationToken);
            if (imageExists)
            {
                await blob.DeleteAsync(cancellationToken);
            }
            await blob.UploadTextAsync(content, cancellationToken);
            return true;
        }
    }
}
