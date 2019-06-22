using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Data;
using PlusValuesFifo.Models;
using PlusValuesFifo.Services;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PlusValuesFifo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlusValuesController : Controller
    {
        private readonly IPlusValuesService _plusValuesService;
        private readonly IDataLoaderService<InputEvent> _dataLoaderService;
        private readonly IDataExporterService<OutputEvent> _dataExporterService;
        private readonly ILogger<PlusValuesController> _logger;

        public PlusValuesController(IPlusValuesService plusValuesService,
            IDataLoaderService<InputEvent> dataLoaderService,
            IDataExporterService<OutputEvent> dataExporterService,
            ILogger<PlusValuesController> logger)
        {
            _plusValuesService = plusValuesService;
            _dataLoaderService = dataLoaderService;
            _dataExporterService = dataExporterService;
            _logger = logger;
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
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("file not selected");

            if (!file.FileName.EndsWith(".csv"))
                return BadRequest("only CSV is supported");

            string fileContent = string.Empty;
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                fileContent = await streamReader.ReadToEndAsync();
            }

            if(fileContent.Length == 0)
            {
                _logger.LogError("The file content length is 0 whereas it had not been filtered by file.Length of IFileForm");
                return BadRequest("file content before parsing was 0");
            }

            // Read file and parse it.
            if(_dataLoaderService.TryLoadData(fileContent))
            {
                // If it works, then compute plusvalues with imported content
                var events = _dataLoaderService.GetEvents();

                if (_plusValuesService.TryComputePlusValues(events, out var outputs))
                {
                    string outputContent = string.Empty;
                    if (_dataExporterService.TryExportData(outputs, out outputContent))
                    {
                        var bytes = Encoding.UTF8.GetBytes(outputContent);
                        return File(bytes, "text/csv", "PlusValues.csv");
                    }
                    else
                        return BadRequest("Error whilst exporting data output");
                }
                else
                    return BadRequest("Error whilst plusvalues computation");
            }
            else
                return BadRequest("Error whilst parsing data. values have to be separated with a ;");
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
