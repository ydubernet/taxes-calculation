using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Data;
using PlusValuesFifo.Models;
using PlusValuesFifo.Services;

namespace PlusValuesFifo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddConsole()
                              .AddDebug()
                              .AddEventSourceLogger()
            );

            services.AddSingleton<IParser<IEvent>>(new CsvParser<IEvent>());

            services.AddSingleton<IDataLoader<IEvent>>((sp) =>
                new DataLoader<IEvent>(sp.GetService<IParser<IEvent>>(),
                                       Configuration.GetValue<string>("InputFilePath"),
                                       sp.GetService<ILogger>()));

            services.AddSingleton<IDataExporter<IEvent>>((sp) =>
                new DataExporter<IEvent>(sp.GetService<IFileGenerator<IEvent>>(),
                                         Configuration.GetValue<string>("OutputFilePath"),
                                         sp.GetService<ILogger>()));

            services.AddSingleton<IPlusValuesService, PlusValuesService>();

            services.AddHealthChecks();
            services.AddMvc(config => config.ReturnHttpNotAcceptable = true)
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseHealthChecks("/monitoring/status");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
