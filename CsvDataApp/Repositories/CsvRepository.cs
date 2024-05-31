using CsvDataApp.Data;
using CsvDataApp.Helpers.CsvHelper;
using CsvDataApp.Models;
using CsvHelper;
using System.Globalization;
using System;

namespace CsvDataApp.Repositories
{
    public class CsvRepository : ICsvRepository
    {
        private readonly AppDbContext _context;
        public CsvRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Person>, List<Dictionary<string, string>>)?> ImportCsv(IFormFile file)
        {
            var records = new List<Person>();
            var badRecords = new List<Dictionary<string, string>>();

            if (file == null || file.Length <= 0)
            {
                return null; 
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<PersonMap>();

                csv.Read();
                csv.ReadHeader();
                var headers = csv.Context.Reader.HeaderRecord;

                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<Person>();
                        records.Add(record);
                    }
                    catch (Exception ex)
                    {
                        var badRecord = new Dictionary<string, string>();
                        foreach (var header in headers)
                        {
                            badRecord[header] = csv.GetField(header);
                        }
                        badRecord["Error"] = ex.Message;
                        badRecords.Add(badRecord);
                    }
                }

                if (badRecords.Any())
                {
                    var badRecordsPath = Path.Combine(Directory.GetCurrentDirectory(), "badRecords.csv");
                    using (var writer = new StreamWriter(badRecordsPath))
                    using (var badCsv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        // Write headers including an Error column
                        foreach (var header in headers)
                        {
                            badCsv.WriteField(header);
                        }
                        badCsv.WriteField("Error");
                        badCsv.NextRecord();

                        // Write bad records
                        foreach (var badRecord in badRecords)
                        {
                            foreach (var header in headers)
                            {
                                badCsv.WriteField(badRecord[header]);
                            }
                            badCsv.WriteField(badRecord["Error"]);
                            badCsv.NextRecord();
                        }
                    }
                }

                if (records.Any())
                {
                    _context.Persons.AddRange(records);
                    await _context.SaveChangesAsync();
                }
            }

            return (records, badRecords);
        }

        public async Task<byte[]> DownloadBadRecordCsv()
        {
            var badRecordsPath = Path.Combine(Directory.GetCurrentDirectory(), "badRecords.csv");
            if (!System.IO.File.Exists(badRecordsPath))
            {
                return null;
            }

            var bytes = System.IO.File.ReadAllBytes(badRecordsPath);

            return bytes;
        }

        
    }

    public interface ICsvRepository
    {
        Task<(List<Person>, List<Dictionary<string, string>>)?> ImportCsv(IFormFile file);
        Task<byte[]> DownloadBadRecordCsv();
    }
}
