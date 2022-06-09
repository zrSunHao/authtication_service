using Hao.Authentication.Domain.Consts;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Manager.Basic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;

namespace Hao.Authentication.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceManager _manager;
        protected readonly IConfiguration _configuration;

        public ResourceController(IResourceManager manager,
            IConfiguration configuration)
        {
            _manager = manager;
            _configuration = configuration;
        }

        [HttpPost("Save")]
        public async Task<ResponseResult<string>> Save(string ownerId,string category)
        {
            var res = new ResponseResult<string>();
            try
            {
                if (string.IsNullOrEmpty(ownerId)) throw new MyCustomException("ownerId is null!");
                if (string.IsNullOrEmpty(category)) throw new MyCustomException("category is null!");
                var invalid = !Request.HasFormContentType || Request.Form.Files == null || !Request.Form.Files.Any();
                if (invalid) throw new MyCustomException("No file found in the form!");
                var file = Request?.Form?.Files?.FirstOrDefault();
                if (file == null) throw new MyCustomException("No file found in the form!");

                string rootPath = _configuration[CfgConsts.FILE_RESOURCE_DIRECTORY];
                var directory = new DirectoryInfo(rootPath);
                if (!directory.Exists) { directory.Create(); }

                var code = _manager.GetNewCode();
                var suffix = Path.GetExtension(file.FileName);
                var newName = file.FileName;
                var newFileName = code + suffix;
                var fp = @$"{rootPath}\{newFileName}";

                using (var fs = new FileStream(fp, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fs);
                }

                var model = new ResourceM()
                {
                    Code = code,
                    Name = file.FileName,
                    OwnId = ownerId,
                    FileName = newFileName,
                    Type = file.ContentType,
                    Suffix = suffix,
                    Length = file.Length,
                    Category = category
                };

                var result = await _manager.Save(model);
                if (result.Success) res.Data = _manager.BuilderFileUrl(model.FileName);
                else new MyCustomException(result.AllMessages);
            }
            catch (Exception e)
            {
                res.AddError(e);
            }
            return res;
        }

        [HttpGet("GetByCode")]
        public async Task<ResponseResult<ResourceM>> GetByCode(string code)
        {
            return await _manager.GetByCode(code);
        }

        [AllowAnonymous]
        [HttpGet("GetFileByName")]
        public IActionResult GetFileByName(string name,string key)
        {
            try
            {
                string rootPath = _configuration[CfgConsts.FILE_RESOURCE_DIRECTORY];
                string path = @$"{rootPath}\{name}";
                var file = new FileInfo(path);
                if (!file.Exists) return NotFound();

                var suffix = Path.GetExtension(name);
                var provider = new FileExtensionContentTypeProvider();
                var memi = provider.Mappings[suffix]; // 获取文件类型
                string? type = new MediaTypeHeaderValue(memi).MediaType;
                type = type == null ? "" : type;
                FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
                return File(fs, contentType: type, file.Name, enableRangeProcessing: true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
