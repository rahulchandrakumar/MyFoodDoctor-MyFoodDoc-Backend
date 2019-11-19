using Microsoft.Azure.Storage;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using MyFoodDoc.CMS.Infrastructure.AzureBlob.Implementation;
using System;

namespace MyFoodDoc.CMS.Infrastructure.Dependencyinjection
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddAzureStorage(this IServiceCollection services, string ConnectionString, Uri CDN)
        {
            ImageBlobService.ConnectionString = ConnectionString;
            ImageBlobService.ContainerName = "images";
            ImageBlobService.CDN = CDN;
            ImageBlobService.OriginalUrl = CloudStorageAccount.Parse(ConnectionString).BlobStorageUri.PrimaryUri;

            services.AddScoped<IImageBlobService, ImageBlobService>();

            return services;
        }
    }
}
