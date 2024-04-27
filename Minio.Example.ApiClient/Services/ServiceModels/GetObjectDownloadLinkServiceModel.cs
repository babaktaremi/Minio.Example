namespace Minio.Example.ApiClient.Services.ServiceModels;

public record GetObjectDownloadLinkServiceModel(string DownloadLink);

public record GetObjectDownloadLinkRequestModel(string FileName,string BucketName,TimeSpan ExpiryDuration);