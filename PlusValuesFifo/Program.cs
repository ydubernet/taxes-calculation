using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Data;
using PlusValuesFifo.Services;

namespace PlusValuesFifo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var csvParser = new CsvParser();
            var dataLoader = new DataLoader(csvParser, @"Transactions.csv");
            var plusValuesService = new PlusValuesService(dataLoader);

            plusValuesService.TryComputePlusValues();

            Console.WriteLine("End of execution");
            Console.ReadKey();

            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
