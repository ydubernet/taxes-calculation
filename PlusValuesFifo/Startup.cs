using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Data;
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

            services.AddSingleton<IParser>(new CsvParser());

            services.AddSingleton<IDataLoader>((sp) =>
                new DataLoader(sp.GetService<IParser>(),
                Configuration.GetValue<string>("InputFilePath"),
                sp.GetService<ILogger>()));

            services.AddSingleton<IPlusValuesService>((sp) =>
                new PlusValuesService(sp.GetService<IDataLoader>()));

            services.AddHealthChecks();
            services.AddMvc()
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseHealthChecks("/monitoring/status");
            app.UseMvc();
        }
    }
}
