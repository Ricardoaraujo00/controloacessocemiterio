using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.IO;
using cemiteryapp.Server.Hubs;
using System.Collections.Generic;
using System;

namespace cemiteryapp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var rootDirectoryFile = env.ContentRootPath+"\\data.txt";
            try
            {
                if (!File.Exists(rootDirectoryFile))
                {
                    using (StreamWriter streamWriter = new StreamWriter(rootDirectoryFile, false))
                    {
                        streamWriter.WriteLine("actual|0");
                        streamWriter.WriteLine("max|150");
                        SignalRHub.numActual = 0;
                        SignalRHub.numMaximo = 150;
                    }
                }
                else
                {
                    Dictionary<string, int> initialData = new Dictionary<string, int>();

                    using (StreamReader sr = new StreamReader(rootDirectoryFile))
                    {
                        // Read the stream to a string, and write the string to the console.
                        while (!sr.EndOfStream)
                        {
                            string readedLine = sr.ReadLine();
                            string[] dataArray = new string[2];
                            dataArray = readedLine.Split('|');
                            initialData[dataArray[0]] = Int32.Parse(dataArray[1]);
                        }
                    }

                    SignalRHub.numActual = initialData["actual"];
                    SignalRHub.numMaximo = initialData["max"]; 
                }
            }
            catch
            {
                SignalRHub.numActual = 0;
                SignalRHub.numMaximo = 55;
            }
            
            



            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapHub<SignalRHub>("/SignalRHub");
            });
        }
    }
}
