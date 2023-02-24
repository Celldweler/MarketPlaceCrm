using System.IO;
using System.Threading.Tasks;
using MarketPlaceCrm.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Route("api/files")]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _ctx;

        public FileController(IWebHostEnvironment env, AppDbContext ctx)
        {
            _env = env;
            _ctx = ctx;
        }

        // get file by fileName
        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest();

            var savePath = Path.Combine(_env.WebRootPath, fileName);
            if (!System.IO.File.Exists(savePath))
                return NotFound("file not exist");

            return new FileStreamResult(new FileStream(savePath, FileMode.Open, FileAccess.Read), "image/*");
        }

        public class FileForm
        {
            public string FileName { get; set; }
            public IFormFile FormFile { get; set; }
        }

        // upload file 
        [HttpPost]
        public async Task<IActionResult> DownloadFile(IFormFile file)
        {
            if (file == null) return BadRequest("object file equal null");

            await using (var stream = new FileStream(Path.Combine(_env.WebRootPath, file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(file.FileName);
        }
    }
}