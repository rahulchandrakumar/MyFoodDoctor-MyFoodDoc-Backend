using Microsoft.Azure.Storage;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Seed;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using MyFoodDoc.CMS.Infrastructure.AzureBlob.Implementation;
using System;

namespace MyFoodDoc.CMS.Infrastructure
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddAzureStorage(this IServiceCollection services, string ConnectionString, Uri CDN)
        {
            ImageBlobService.ConnectionString = ConnectionString;
            WebViewBlobService.ConnectionString = ConnectionString;

            var cloudStorageUrl = CloudStorageAccount.Parse(ConnectionString).BlobStorageUri.PrimaryUri;

            ImageModel.CdnUrl = CDN;
            ImageModel.OriginalUrl = cloudStorageUrl;

            WebViewModel.CdnUrl = CDN;
            WebViewModel.OriginalUrl = cloudStorageUrl;

            WebPageSeed.OriginalUrl = cloudStorageUrl;

            services.AddScoped<IImageBlobService, ImageBlobService>();
            services.AddScoped<IWebViewBlobService, WebViewBlobService>();

            return services;
        }

        public static IServiceCollection AddSeeds(this IServiceCollection services)
        {
            services.AddSingleton<ISeed, WebPageSeed>();

            return services;
        }
    }
}
