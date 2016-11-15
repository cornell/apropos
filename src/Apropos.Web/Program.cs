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
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Apropos.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.ObjectPool;
using System.Threading;

namespace Apropos.Web
{
    public class Program
    {
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
            var services = new ServiceCollection();
            services.AddTransient<RazorViewToStringRenderer>();

            var articles = service.GetArticles(Axe.Formation);
            articles.ForEach(article =>
            {
                CreerFormationsPdf(article, env, logger);
            });            
        }

        private static void CreerFormationsPdf(Article article, IHostingEnvironment env, ILogger logger)
        {
            DirectoryInfo repertoire = CreerRepertoirePdf(article, env, logger);

            IServiceScopeFactory serviceScopeFactory = InitializeServices();
            ContratFormationView viewModel;
            if (article.HasFinancementDpc)
            {
                viewModel = ContratFormationView.Create(article, Financement.Dpc);
                CreerContratFormationPdf(serviceScopeFactory, repertoire, logger, viewModel, "contrat-formation-dpc", "Contrats/ContratFormationDpcHorsDpc");
                Thread.Sleep(2000);
            }
            if (article.HasFinancementHorsDpc)
            {
                viewModel = ContratFormationView.Create(article, Financement.HorsDpc);
                CreerContratFormationPdf(serviceScopeFactory, repertoire, logger, viewModel, "contrat-formation-hors-dpc", "Contrats/ContratFormationDpcHorsDpc");
                Thread.Sleep(2000);
            }
            if (article.HasFinancementSalarie)
            {
                viewModel = ContratFormationView.Create(article, Financement.Salarie);
                CreerContratFormationPdf(serviceScopeFactory, repertoire, logger, viewModel, "contrat-formation-salarie", "Contrats/ContratFormationSalarie");
            }
        }

        private static void CreerContratFormationPdf(IServiceScopeFactory serviceScopeFactory, DirectoryInfo repertoire, ILogger logger, ContratFormationView viewModel, string filename, string template)
        {
            var modeleHtml = RenderViewAsync(serviceScopeFactory, viewModel, template).Result;
            string cheminModeleHtml = $"{repertoire.FullName}/{filename}.html";
            using (StreamWriter DestinationWriter = File.CreateText(cheminModeleHtml))
            {
                try
                {
                    DestinationWriter.Write(modeleHtml);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Erreur sur la création du template PDF: {cheminModeleHtml}", ex);
                }
            }
        }

        private static DirectoryInfo CreerRepertoirePdf(Article article, IHostingEnvironment env, ILogger logger)
        {
            string path = "";
            DirectoryInfo result = null;
            try
            {
                string sAppPath = env.ContentRootPath;
                string swwwRootPath = env.WebRootPath;
                path = env.WebRootPath + "/formation/" + article.Url;
                result = Directory.CreateDirectory(path);
            }
            catch(Exception ex)
            {
                logger.LogError($"Erreur sur la création du chemin: '{path}'", ex);
            }
            return result;
        }

        public static Task<string> RenderViewAsync<T>(IServiceScopeFactory scopeFactory, T viewModel, string template)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var helper = serviceScope.ServiceProvider.GetRequiredService<RazorViewToStringRenderer>();
                return helper.RenderViewToStringAsync(template, viewModel);
            }
        }

        public static IServiceScopeFactory InitializeServices(string customApplicationBasePath = null)
        {
            // Initialize the necessary services
            var services = new ServiceCollection();
            ConfigureDefaultServices(services, customApplicationBasePath);

            // Add a custom service that is used in the view.
            //services.AddSingleton<EmailReportGenerator>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IServiceScopeFactory>();
        }

        private static void ConfigureDefaultServices(IServiceCollection services, string customApplicationBasePath)
        {
            var applicationEnvironment = PlatformServices.Default.Application;
            string applicationName;
            IFileProvider fileProvider;
            if (!string.IsNullOrEmpty(customApplicationBasePath))
            {
                applicationName = Path.GetFileName(customApplicationBasePath);
                fileProvider = new PhysicalFileProvider(customApplicationBasePath);
            }
            else
            {
                applicationName = applicationEnvironment.ApplicationName;
                fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            }

            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
            {
                ApplicationName = applicationName,
                WebRootFileProvider = fileProvider,
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(fileProvider);
            });
            var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddLogging();
            services.AddMvc();
            services.AddTransient<RazorViewToStringRenderer>();
        }
    }
}