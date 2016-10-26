using Apropos.Domain;
using Apropos.Web.Controllers;
using Apropos.Web.Infrastructure;
using Apropos.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Apropos.Web
{
    public class Startup
    {
        IHostingEnvironment _environment;

        public Startup(IHostingEnvironment env)
        {
            _environment = env;
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //     .Enrich.FromLogContext() // ensures that any events written directly through Serilog will seamlessly pick up correlation ids like RequestId from ASP.NET.
            //    .Enrich.WithMachineName()
            //    .Enrich.WithEnvironmentUserName()
            //    .Enrich.WithProperty("Application", "apropos")
            //    //.WriteTo.LiterateConsole()
            //    .WriteTo.Seq("http://locahost:5341")
            //    .CreateLogger();

            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .Enrich.FromLogContext()
            //    .Enrich.WithMachineName()
            //    .Enrich.WithEnvironmentUserName()
            //    .Enrich.WithProperty("Application", "apropos.console")
            //    .Enrich.With(new ThreadIdEnricher())
            //    .WriteTo.LiterateConsole()
            //    .WriteTo.RollingFile("logs\\myapp-{Date}.txt")
            //    .WriteTo.Seq("http://localhost:5341")
            //    .CreateLogger();

            // Configure the Serilog pipeline
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext() // ensures that any events written directly through Serilog will seamlessly pick up correlation ids like RequestId from ASP.NET.
                .Enrich.WithProperty("Application", "apropos.web")
                //.WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ArticleService,ArticleService>();
            services.AddSingleton<ITodoRepository, TodoRepository>();
            services.AddSingleton<ArticleRepository, ArticleRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Add Serilog to the logging pipeline
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "prevention",
                //    template: "prevention/article/*{url}");

                //routes.MapRoute(
                //    name: "recherche",
                //    template: "recherche/article/*{url}");

                //routes.MapRoute(
                //    name: "formation",
                //    template: "{controller=formation}/{action=article}/{url}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        //    app.Run(async (context) =>
        //    {
        //        await context.Response.WriteAsync("Hello World!");
        //    });
        //}
    }
}
