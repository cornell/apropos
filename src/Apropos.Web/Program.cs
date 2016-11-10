using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using Apropos.Domain;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Apropos.Core;

namespace Apropos.Web
{

    public class Program
    {
        private static PhysicalFileProvider _fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            ArticleService service = host.Services.GetService(typeof(ArticleService)) as ArticleService;
            IHostingEnvironment env = host.Services.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            ILogger logger = host.Services.GetService(typeof(ILogger)) as ILogger;
            StartupDomainApplication(service, env, logger); 

            host.Run();
        }

        private static void StartupDomainApplication(ArticleService service, IHostingEnvironment env, ILogger logger)
        {
            var articles = service.GetArticles(Axe.Formation);
            articles.ForEach(article =>
            {
                DirectoryInfo repertoire = CreerRepertoirePdf(article, env, logger);
                //RazorViewToStringRenderer render = new RazorViewToStringRenderer();
            });
            
        }

        private static DirectoryInfo CreerRepertoirePdf(Article article, IHostingEnvironment env, ILogger logger)
        {
            string path = "";
            DirectoryInfo result = null;
            try
            {
                string sAppPath = env.ContentRootPath;
                string swwwRootPath = env.WebRootPath;
                path = env.WebRootPath + "/formations/articles/" + article.Url;
                result = Directory.CreateDirectory(path);
            }
            catch(Exception ex)
            {
                logger.LogError($"Erreur sur la création du chemin: '{path}'", ex);
            }
            return result;
        }
    }
}