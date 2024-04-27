using System.ComponentModel.DataAnnotations;

namespace Minio.Example.ApiClient.Services.ServiceModels;

public record UploadFileServiceModel([Base64String] string FileContent,string FileType,string Bucket);
public record UploadFileServiceModelResult(string FileName,string BucketName);