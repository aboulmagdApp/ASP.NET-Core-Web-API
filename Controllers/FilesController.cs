using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cityInfo.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            // Validate the input. Put a limit on filesize to avoid large uploads attacks. 
            // Only accept .pdf files (check content-type)
            if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("No file or an invalid one has been inputted.");
            }
            var path = Path.Combine(
                Directory.GetCurrentDirectory(), $"Upload/{file.FileName}_{Guid.NewGuid()}.pdf");

            using(var stream = new FileStream(path, FileMode.Create) ) 
            { 
                await file.CopyToAsync(stream);
            }
            var filePath = $"{Request.Scheme}://{Request.Host}/{path.Replace(Directory.GetCurrentDirectory() + "\\", "")}";
            return Ok(new { file.FileName, filePath });
        }
    }
}
