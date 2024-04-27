using Minio.Example.ApiClient.Services.Implementations;
using Minio.Example.ApiClient.Services.Interfaces;

namespace Minio.Example.ApiClient.Services.Extensions;

public static class MinioServiceCollectionExtensions
{
    public static IServiceCollection AddStorageService(this IServiceCollection services)
    {
        services.AddMinio(client =>
        {
            client.WithCredentials("fileapi", "FileApi@123")
                .WithEndpoint("localhost:9000")
                .WithSSL(false);
        });

        services.AddScoped<IStorageService, StorageService>();
        
        return services;
    }
}