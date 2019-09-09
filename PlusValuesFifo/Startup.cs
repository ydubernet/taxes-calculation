﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Data;
using PlusValuesFifo.Models;
using PlusValuesFifo.ServiceProviders;

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
            services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
            services.AddSingleton<IMapProvider<IEvent>>();
            services.AddSingleton<IParser<IInputEvent>>(sp => new CsvParser<IInputEvent>(sp.GetService<ILoggerProvider>(), sp.GetService<IMapProvider<IInputEvent>>()));
            services.AddSingleton<IFileGenerator<IOutputEvent>>(sp => new CsvGenerator<IOutputEvent>(sp.GetService<ILoggerProvider>(), sp.GetService<IMapProvider<IOutputEvent>>()));

            services.AddSingleton<IDataLoaderService<IInputEvent>>((sp) =>
                new DataLoaderService<IInputEvent>(sp.GetService<IParser<IInputEvent>>(),
                                                   sp.GetService<ILoggerFactory>()));

            services.AddSingleton<IDataExporterService<IOutputEvent>>((sp) =>
                new DataExporterService<IOutputEvent>(sp.GetService<IFileGenerator<IOutputEvent>>(),
                                                      sp.GetService<ILoggerFactory>()));

            services.AddSingleton<IPlusValuesServiceProvider, PlusValuesServiceProvider>();

            services.AddHealthChecks();
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseHealthChecks("/monitoring/status");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
