using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Data;
using PlusValuesFifo.Models;
using PlusValuesFifo.Services;
using System.IO;
using System.Linq;
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

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        [Produces("text/csv")]
        //https://www.c-sharpcorner.com/article/upload-download-files-in-asp-net-core-2-0/
        public async Task<IActionResult> Post(string fileContent)
        {
            if (fileContent == null || fileContent.Length == 0)
                return BadRequest("file not selected");

            //if (!file.FileName.EndsWith(".csv"))
            //    return BadRequest("only CSV is supported");

            //string fileContent = string.Empty;
            //using (var streamReader = new StreamReader(file.OpenReadStream()))
            //{
            //    fileContent = await streamReader.ReadToEndAsync();
            //}

            if (fileContent.Length == 0)
            {
                _logger.LogError("The file content length is 0 whereas it had not been filtered by file.Length of IFileForm");
                return BadRequest("file content before parsing was 0");
            }

            // Read file and parse it.
            if (_dataLoaderService.TryLoadData(fileContent))
            {
                // If it works, then compute plusvalues with imported content
                var events = _dataLoaderService.GetEvents();
                var outputs = _plusValuesService.ComputePlusValues(events);

                string outputContent = string.Empty;

                // We want our CSV exported file to contain all buying entries and selling entries
                // TODO : That logic shouldn't be in a Controller
                var outputsToExport = OutputDataFormaterHelper.ConcatAllEvents(
                                                                events.Where(e => e.ActionEvent == BuySell.Buy).ToList(),
                                                                outputs);

                if (_dataExporterService.TryExportData(outputsToExport, out outputContent))
                {
                    var bytes = Encoding.UTF8.GetBytes(outputContent);
                    return File(bytes, "text/csv", "PlusValues.csv");
                }
                else
                {
                    return BadRequest("Error whilst exporting data output");
                }
            }
            else
            {
                return BadRequest("Error whilst parsing data. values have to be separated with a ;");
            }
        }
    }
}
