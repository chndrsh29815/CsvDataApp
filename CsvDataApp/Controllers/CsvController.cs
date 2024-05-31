using CsvDataApp.Data;
using CsvDataApp.Helpers.CsvHelper;
using CsvDataApp.Models;
using CsvDataApp.Repositories;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CsvDataApp.Controllers
{
    public class CsvController : Controller
    {
        private readonly ICsvRepository _repository;
        public CsvController(ICsvRepository repository)
        {
            _repository = repository;
        }
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var result = await _repository.ImportCsv(file);
            if(result == null)
            {
                return BadRequest();
            }
            TempData["ImportSummary"] = $"Imported {result.Value.Item1.Count} records successfully. {result.Value.Item2.Count} records failed.";
            if (result.Value.Item2.Count > 0)
            {
                TempData["DownloadLink"] = Url.Action("DownloadBadRecords");
            }
            return RedirectToAction("Index", "Person");
        }


        [HttpGet]
        public async Task<IActionResult> DownloadBadRecords()
        {
            var bytes = await _repository.DownloadBadRecordCsv();
            return File(bytes, "application/octet-stream", "badRecords.csv");
        }
    }


}
