using FileTypeChecker;
using Minio.DataModel.Args;
using Minio.Example.ApiClient.Services.Interfaces;
using Minio.Example.ApiClient.Services.ServiceModels;

namespace Minio.Example.ApiClient.Services.Implementations;

public class StorageService:IStorageService
{
    private readonly IMinioClient _minioClient;

    public StorageService(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task CreateBucketAsync(CreateBucketServiceModel createBucketModel,CancellationToken cancellationToken=default)
    {
        var createBucketArgs = new MakeBucketArgs().WithBucket(createBucketModel.Name);

        await _minioClient.MakeBucketAsync(createBucketArgs, cancellationToken);
    }

    public async Task<UploadFileServiceModelResult> UploadBase64FileAsync(UploadFileServiceModel uploadFileServiceModel,
        CancellationToken cancellationToken = default)
    {
        await using var ms = new MemoryStream(Convert.FromBase64String(uploadFileServiceModel.FileContent));

        var fileName = $"{Guid.NewGuid():N}.{FileTypeValidator.GetFileType(ms).Extension}";

        ms.Position = 0;

        var createFileArgs = new PutObjectArgs()
            .WithBucket(uploadFileServiceModel.Bucket)
            .WithStreamData(ms)
            .WithObjectSize(ms.Length)
            .WithObject(fileName)
            .WithContentType(!string.IsNullOrEmpty(uploadFileServiceModel.FileType)
                ? uploadFileServiceModel.FileType
                : "application/octet-stream");

        var response=await _minioClient.PutObjectAsync(createFileArgs, cancellationToken: cancellationToken);

        return new UploadFileServiceModelResult(response.ObjectName, uploadFileServiceModel.Bucket);
    }

    public async Task<GetObjectDownloadLinkServiceModel> GetObjectDownloadLink(GetObjectDownloadLinkRequestModel requestModel,
        CancellationToken cancellationToken = default)
    {
        var downloadLinkArgs = new PresignedGetObjectArgs()
            .WithBucket(requestModel.BucketName)
            .WithObject(requestModel.FileName)
            .WithExpiry((int)requestModel.ExpiryDuration.TotalSeconds);

        var downloadLinkRequest = await _minioClient.PresignedGetObjectAsync(downloadLinkArgs);

        return new GetObjectDownloadLinkServiceModel(downloadLinkRequest);
    }
}