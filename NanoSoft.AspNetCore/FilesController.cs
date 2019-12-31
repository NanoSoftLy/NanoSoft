using Microsoft.AspNetCore.Mvc;
using NanoSoft.IO;
using System.IO;
using System.Linq;

namespace NanoSoft.AspNetCore
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{directory}/{file}")]
        public IActionResult Get(string directory, string file)
        {
            var path = Path.Combine(_fileService.SavePath, directory, file);

            if (!System.IO.File.Exists(path))
                return NotFound();

            var stream = System.IO.File.OpenRead(path);

            var extention = file.Split('.').LastOrDefault();
            var type = "image/" + extention;

            switch (extention)
            {
                case "pdf":
                    type = "application/pdf";
                    break;

                case "doc":
                    type = "application/word";
                    break;

                case "docx":
                    type = "application/word";
                    break;
            }

            return File(stream, type);
        }
    }
}
