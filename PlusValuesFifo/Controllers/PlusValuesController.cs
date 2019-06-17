using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlusValuesFifo.Data;
using PlusValuesFifo.Models;
using PlusValuesFifo.Services;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PlusValuesFifo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlusValuesController : Controller
    {
        private readonly IPlusValuesService _plusValuesService; // TODO : It should only compute the plusValues, not retrieve data
        private readonly IDataExporter<IEvent> _dataExporter; // TODO : That should be in a dedicated service

        public PlusValuesController(IPlusValuesService plusValuesService, IDataExporter<IEvent> dataExporter)
        {
            _plusValuesService = plusValuesService;
            _dataExporter = dataExporter;
        }

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

            // Missing step : write file on the server (maybe not, depending on what we wanna do, but implies few more changes)

            _plusValuesService.TryComputePlusValues(out var outputs);
            _dataExporter.TryExportData(outputs); // TODO : For the moment, it writes the file and returns OK. We may wanna return the file at Controller level.

            return Ok(); //File(bytes, "text/csv");
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
