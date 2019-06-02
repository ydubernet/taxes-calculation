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
            //services.AddLogging();

            services.AddSingleton<IParser>(new CsvParser());

            services.AddSingleton<IDataLoader>((sp) =>
                new DataLoader(sp.GetService<IParser>(),
                Configuration.GetValue<string>("InputFilePath"),
                sp.GetService<ILogger>()));

            services.AddSingleton<IPlusValuesService>((sp) =>
                new PlusValuesService(sp.GetService<IDataLoader>()));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            loggerFactory.AddEventSourceLogger();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
