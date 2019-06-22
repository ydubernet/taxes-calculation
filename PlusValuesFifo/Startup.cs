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
            services.AddSingleton<IParser<InputEvent>>(new CsvParser<InputEvent>());
            services.AddSingleton<IFileGenerator<OutputEvent>>(new CsvGenerator<OutputEvent>());

            services.AddSingleton<IDataLoaderService<InputEvent>>((sp) =>
                new DataLoaderService<InputEvent>(sp.GetService<IParser<InputEvent>>(),
                                              sp.GetService<ILogger<DataLoaderService<InputEvent>>>()));

            services.AddSingleton<IDataExporterService<OutputEvent>>((sp) =>
                new DataExporterService<OutputEvent>(sp.GetService<IFileGenerator<OutputEvent>>(),
                                                sp.GetService<ILogger<DataExporterService<OutputEvent>>>()));

            services.AddSingleton<IPlusValuesService>((sp) =>
                new PlusValuesService(sp.GetService<ILogger<PlusValuesService>>()));

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
