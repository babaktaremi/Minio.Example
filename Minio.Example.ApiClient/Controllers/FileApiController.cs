using Microsoft.AspNetCore.Mvc;
using Minio.Example.ApiClient.Services.Interfaces;
using Minio.Example.ApiClient.Services.ServiceModels;

namespace Minio.Example.ApiClient.Controllers;

[ApiController]
[Route("FileApi")]
public class FileApiController : ControllerBase
{
    private readonly IStorageService _storageService;

    public FileApiController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost("CreateBucket")]
    public async Task<IActionResult> CreateBucket(CreateBucketServiceModel model,CancellationToken cancellationToken)
    {
        await _storageService.CreateBucketAsync(model, cancellationToken);

        return Created();
    }

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile(UploadFileServiceModel model, CancellationToken cancellationToken)
    {
        var result = await _storageService.UploadBase64FileAsync(model, cancellationToken);

        return Created(Url.Action("GetDownloadloadLink",
            new { filename = result.FileName, bucketName = result.BucketName }),null);
    }

    [HttpGet("DownloadLink")]
    public async Task<IActionResult> GetDownloadloadLink(string fileName, string bucketName)
    {
        var downloadLink =
            await _storageService.GetObjectDownloadLink(
                new GetObjectDownloadLinkRequestModel(fileName, bucketName, TimeSpan.FromSeconds(30)));

        return Ok(downloadLink);
    }
}