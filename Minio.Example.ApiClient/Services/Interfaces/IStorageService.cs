using Minio.Example.ApiClient.Services.ServiceModels;

namespace Minio.Example.ApiClient.Services.Interfaces;

public interface IStorageService
{
    Task CreateBucketAsync(CreateBucketServiceModel createBucketModel,CancellationToken cancellationToken=default);

    Task<UploadFileServiceModelResult> UploadBase64FileAsync(UploadFileServiceModel uploadFileServiceModel,
        CancellationToken cancellationToken = default);

    Task<GetObjectDownloadLinkServiceModel> GetObjectDownloadLink(GetObjectDownloadLinkRequestModel requestModel,
        CancellationToken cancellationToken = default);

}