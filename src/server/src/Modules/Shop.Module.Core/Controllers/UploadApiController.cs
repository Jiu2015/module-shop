using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Infrastructure;
using Shop.Module.Core.Services;
using System.Net.Http.Headers;

namespace Shop.Module.Core.Controllers
{
    /// <summary>
    /// 上传服务相关 API
    /// </summary>
    [ApiController]
    [Route("api/upload")]
    [Authorize()] //Roles = "admin"
    public class UploadApiController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly IStorageService _storageService;

        public UploadApiController(
            IMediaService mediaService,
            IStorageService storageService)
        {
            _mediaService = mediaService;
            _storageService = storageService;
        }

        /// <summary>
        /// 单文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<Result> Upload(IFormFile file)
        {
            if (file == null)
            {
                return Result.Fail("Please select upload file");
            }
            var headerValue = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var originalFileName = headerValue.FileName.Trim('"');
            var fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(originalFileName)}";

            var result = await _mediaService.SaveMediaAsync(file.OpenReadStream(), fileName, file.ContentType, file.Length);

            var url = await _storageService.GetMediaUrl(result.FileName);
            return Result.Ok(new
            {
                result.Id,
                result.FileName,
                url
            });
        }

        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("multiple")]
        public async Task<Result> MultiUpload(IFormCollection formCollection)
        {
        }
    }
}
