using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PlusValuesFifo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlusValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[Consumes("text/csv")]
        [Produces("text/csv")]
        //https://www.c-sharpcorner.com/article/upload-download-files-in-asp-net-core-2-0/
        public IActionResult Post(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("file not selected");

            var bytes = Encoding.UTF8.GetBytes("hello;world");
            return File(bytes, "text/csv");
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
                {
                    {".txt", "text/plain"},
                    {".pdf", "application/pdf"},
                    {".doc", "application/vnd.ms-word"},
                    {".docx", "application/vnd.ms-word"},
                    {".xls", "application/vnd.ms-excel"},
                    {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                    {".png", "image/png"},
                    {".jpg", "image/jpeg"},
                    {".jpeg", "image/jpeg"},
                    {".gif", "image/gif"},
                    {".csv", "text/csv"}
                };
        }
    }
}
